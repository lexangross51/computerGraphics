using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using cg_3.Source.Render;
using cg_3.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using OpenTK.Wpf;
using ReactiveUI;

namespace cg_3.Views.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<PlaneViewModel>
{
    object? IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    private ViewableBezierObject? _bezierObject;
    public PlaneViewModel? ViewModel { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Current.Services.GetRequiredService<PlaneViewModel>();
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        IBaseGraphic baseGraphic = new RenderServer();
        baseGraphic.Camera.AspectRatio =
            (float)(OpenTkControl.RenderSize.Width / OpenTkControl.RenderSize.Height);

        var observable = Observable.FromEvent<TimeSpan>(
            handler => OpenTkControl.Render += handler,
            handler => OpenTkControl.Render -= handler);

        this.WhenActivated(disposables =>
        {
            observable.Subscribe(ts => baseGraphic.Render(ts)).DisposeWith(disposables);

            OpenTkControl.Events().MouseDown.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                if (ViewModel.Mode == Mode.Select)
                {
                    var key = ViewModel.FindWrapper((x, y));
                    if (key == Guid.Empty) return;
                    ViewModel.SelectedWrapper = ViewModel.Wrappers.Lookup(key).Value;
                    ViewModel.DrawSelected(baseGraphic, key);
                    return;
                }

                if (!ViewModel.HaveViewableObject)
                {
                    _bezierObject = new((x, y), ViewModel);
                    ViewModel.HaveViewableObject = true;
                }

                await _bezierObject!.AddPoint((x, y));
                ViewModel.Draw(baseGraphic);

                if (_bezierObject.State != StateViewableObject.Completed) return;
                _bezierObject.Restart((x, y));
            }).DisposeWith(disposables);

            ViewModel.Plane.SelectedPoints.CountChanged.Where(_ => ViewModel.Mode is not Mode.Select)
                .Subscribe(_ => ViewModel.Draw(baseGraphic));

            OpenTkControl.Events().MouseMove.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                MousePositionX.Text = x.ToString("G7", CultureInfo.InvariantCulture);
                MousePositionY.Text = y.ToString("G7", CultureInfo.InvariantCulture);

                if (ViewModel.Plane.SelectedPoints.Count == 0) return;
                if (ViewModel.Mode == Mode.Select)
                {
                    ViewModel.Plane.ClearSelected();
                    ViewModel.Draw(baseGraphic);
                    return;
                }

                await _bezierObject!.MovePoint((x, y));
                _bezierObject.GenerateSegment();
                ViewModel.Draw(baseGraphic);
            }).DisposeWith(disposables);

            this.Events().KeyDown
                .Where(x => x.Key == Key.Z && x.KeyboardDevice.IsKeyDown(Key.LeftCtrl))
                .Subscribe(_ =>
                {
                    ViewModel.Cancel.Execute().Subscribe();
                    ViewModel.Draw(baseGraphic);
                })
                .DisposeWith(disposables);
        });
    }
}