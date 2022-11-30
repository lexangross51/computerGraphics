using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using cg_3.Source.Render;
using cg_3.ViewModels;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
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
            _planeView.Wrappers.Connect().OnItemAdded(wrapper => ViewModel.BezierWrapper = wrapper).Subscribe()
                .DisposeWith(disposables);

            observable.Subscribe(ts => baseGraphic.Render(ts)).DisposeWith(disposables);

            OpenTkControl.Events().MouseDown.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                // if (ViewModel.IsSelectSegmentMode)
                // {
                //     var key = _planeView.FindWrapper((x, y));
                //     if (key == Guid.Empty) return;
                //     baseGraphic.Clear();
                //     baseGraphic.Draw(_planeView.Plane.Curves, PrimitiveType.LineStrip);
                //     baseGraphic.Draw(_planeView.Wrappers.Lookup(key).Value.Points, PrimitiveType.LineStrip,
                //         Color4.Chartreuse);
                //     baseGraphic.DrawPoints(_planeView.Wrappers.Lookup(key).Value.ControlPoints, 7, Color4.Chartreuse);
                //     return;
                // }

                if (!ViewModel.IsDrawingMode) return;

                if (!_planeView.HaveViewableObject)
                {
                    _bezierObject = new((x, y), _planeView);
                    _planeView.HaveViewableObject = true;
                }

                await _bezierObject.AddPoint((x, y));

                if (_bezierObject.State != StateViewableObject.Completed) return;
                _bezierObject.Restart((x, y));
                _planeView.Draw(baseGraphic);
            }).DisposeWith(disposables);

            _planeView.Plane.SelectedPoints.CountChanged.Where(_ => ViewModel.IsDrawingMode)
                .Subscribe(_ => _planeView.Draw(baseGraphic));

            OpenTkControl.Events().MouseMove.Subscribe(async args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                MousePositionX.Text = x.ToString("G7", CultureInfo.InvariantCulture);
                MousePositionY.Text = y.ToString("G7", CultureInfo.InvariantCulture);

                if (_planeView.Plane.SelectedPoints.Count == 0) return;

                await _bezierObject.MovePoint((x, y));
                _bezierObject.GenerateSegment();
                _planeView.Draw(baseGraphic);
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