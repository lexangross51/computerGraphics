using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using cg_3.Source.Render;
using cg_3.ViewModels;
using DynamicData;
using DynamicData.Binding;
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

            MouseDoubleClick += (_, _) => ViewModel.PlaneView.AddWrapper.Execute(new((-0.9f, 0.0f),
                (-0.5f, 0.5f), (0.0f, -0.5f),
                (1.0f, 0.0f))).Subscribe();

            OpenTkControl.MouseWheel += (_, e) => _baseGraphic.Camera.Fov -= e.Delta / 100.0f;
        });
    }
}

#nullable restore