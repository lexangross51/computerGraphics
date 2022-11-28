using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using cg_3.Models;
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
    private ViewableBezierObject _bezierObject;
    private bool _create;

    public MainWindow()
    {
        InitializeComponent();
        ViewModel = App.Current.Services.GetRequiredService<MainViewModel>();
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

            ViewModel.PlaneView.Plane.ControlPoints.CountChanged.Where(c => c % 4 == 0 && c != 0)
                .Subscribe(_ =>
                {
                    ViewModel.PlaneView.Plane.Curves.AddRange(ViewModel.PlaneView.Plane.SelectedCurves);
                    ViewModel.PlaneView.Plane.SelectedCurves.Clear();

                    for (int i = 0; i <= 20; i++)
                    {
                        var t = i / 20.0f;

                        ViewModel.PlaneView.Plane.SelectedCurves.Add(_bezierObject.BezierWrapper.GenCurve(t));
                    }

                    ViewModel.PlaneView.Draw(baseGraphic);
                });

            OpenTkControl.Events().MouseDown.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                if (!_create)
                {
                    _bezierObject = new((x, y), ViewModel.PlaneView);
                    _create = true;
                }

                await _bezierObject.AddPoint((x, y), ref _create);
            }).DisposeWith(disposables);

            ViewModel.PlaneView.Plane.ControlPoints.CountChanged.Subscribe(_ =>
                baseGraphic.DrawPoints(ViewModel.PlaneView.Plane.ControlPoints.Items));

            OpenTkControl.Events().MouseMove.Subscribe(args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                MousePositionX.Text = x.ToString("G7", CultureInfo.InvariantCulture);
                MousePositionY.Text = y.ToString("G7", CultureInfo.InvariantCulture);
            }).DisposeWith(disposables);
        });
    }
}

// OpenTkControl.Events().MouseWheel
//     .Subscribe(args =>
//     {
//         if (args.Delta > 0)
//         {
//             _scale *= 1.1f;
//         }
//         else
//         {
//             _scale *= 0.9f;
//         }
//
//         foreach (var point in ViewModel.PlaneView.Plane.Points)
//         {
//             ViewModel.PlaneView.Plane.ReplacePoint(point * _scale);
//         }
// }).DisposeWith(disposables);

#nullable restore