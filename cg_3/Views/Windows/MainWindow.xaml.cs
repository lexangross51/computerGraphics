using System.Drawing;
using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using cg_3.Source.Render;
using cg_3.ViewModels;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using ReactiveUI;

namespace cg_3.Views.Windows;

#nullable disable

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<PlaneViewModel>
{
    object IViewFor.ViewModel
    {
        get => ViewModel;
        set => throw new NotImplementedException();
    }

    private ViewableBezierObject _bezierObject;
    private readonly PlaneView _planeView = new();
    private bool _create;
    public PlaneViewModel ViewModel { get; set; }

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

            _planeView.Plane.ControlPoints.CountChanged
                .Where(c => c % 4 == 0 && c != 0 && ViewModel.IsDrawingMode)
                .Subscribe(_ =>
                {
                    _planeView.Plane.Curves.AddRange(_planeView.Plane.SelectedCurves);
                    _planeView.Plane.SelectedCurves.Clear();

                    for (int i = 0; i <= 19; i++)
                    {
                        var t = i / 19.0f;

                        _planeView.Plane.SelectedCurves.Add(_bezierObject.BezierWrapper.GenCurve(t));
                    }

                    _planeView.Wrappers.Lookup(_bezierObject.BezierWrapper.Guid).Value.Points
                        .AddRange(_planeView.Plane.SelectedCurves);
                    _planeView.Draw(baseGraphic);
                });

            OpenTkControl.Events().MouseDown.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                if (ViewModel.IsSelectSegmentMode)
                {
                    var key = _planeView.FindWrapper((x, y));
                    if (key == Guid.Empty) return;
                    baseGraphic.Clear();
                    baseGraphic.Draw(_planeView.Plane.Curves, PrimitiveType.LineStrip);
                    baseGraphic.Draw(_planeView.Wrappers.Lookup(key).Value.Points, PrimitiveType.LineStrip,
                        Color4.Chartreuse);
                    baseGraphic.DrawPoints(_planeView.Wrappers.Lookup(key).Value.ControlPoints, 7, Color4.Chartreuse);
                    return;
                }

                if (!ViewModel.IsDrawingMode) return;

                if (!_create)
                {
                    _bezierObject = new((x, y), _planeView);
                    _create = true;
                }

                await _bezierObject.AddPoint((x, y), ref _create);
            }).DisposeWith(disposables);

            _planeView.Plane.ControlPoints.CountChanged.Where(_ => ViewModel.IsDrawingMode).Subscribe(_ =>
                baseGraphic.DrawPoints(_planeView.Plane.ControlPoints.Items, 7, Color4.Black));

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