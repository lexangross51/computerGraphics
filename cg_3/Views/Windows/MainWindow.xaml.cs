using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using cg_3.Source.Render;
using cg_3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OpenTK.Wpf;
using ReactiveUI;

namespace cg_3.Views.Windows;

#nullable disable

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<MainViewModel>
{
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    public MainViewModel ViewModel { get; set; }
    private readonly IBaseGraphic _baseGraphic;

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Current.Services.GetRequiredService<MainViewModel>();
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        _baseGraphic = new RenderServer();
        _baseGraphic.Camera.AspectRatio =
            (float)(OpenTkControl.RenderSize.Width / OpenTkControl.RenderSize.Height);

        var observable = Observable.FromEvent<TimeSpan>(
            handler => OpenTkControl.Render += handler,
            handler => OpenTkControl.Render -= handler);

        this.WhenActivated(disposables =>
        {
            observable.Subscribe(ts => _baseGraphic.Render(ts)).DisposeWith(disposables);
            this.WhenAnyValue(t => t.ViewModel.PlaneView.Wrappers.Count)
                .Subscribe(_ =>
                {
                    foreach (var wrapper in ViewModel.PlaneView.Wrappers)
                    {
                        for (int i = 0; i < 20; i++)
                        {
                            var t = i / 20.0f;
                            ViewModel.PlaneView.Plane.Points.Add(wrapper.GenCurve(t));
                        }
                    }

                    ViewModel.PlaneView.Draw(_baseGraphic);
                }).DisposeWith(disposables);

            OpenTkControl.Events().MouseWheel.Select(args => args)
                .Subscribe(args => _baseGraphic.Camera.Fov -= args.Delta / 100.0f).DisposeWith(disposables);
            
            OpenTkControl.Events().MouseDown.Select(_ => new BezierWrapper(new(-0.9f, 0.0f),
                (-0.5f, 0.5f), (0.0f, -0.5f),
                (1.0f, 0.0f)))
                .InvokeCommand(ViewModel, vm => vm.PlaneView.AddWrapper).DisposeWith(disposables);

            OpenTkControl.Events().MouseMove.Select(args => args).Subscribe(args =>
            {
                var point = args.GetPosition(OpenTkControl);

                MousePositionX.Text = point.X.ToString("G7", CultureInfo.InvariantCulture);
                MousePositionY.Text = point.Y.ToString("G7", CultureInfo.InvariantCulture);
            }).DisposeWith(disposables);
        });
    }
}

#nullable restore