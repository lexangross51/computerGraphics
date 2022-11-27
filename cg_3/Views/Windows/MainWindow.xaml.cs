using System.Globalization;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using cg_3.Models;
using cg_3.Source.Render;
using cg_3.Source.Vectors;
using cg_3.ViewModels;
using DynamicData;
using Microsoft.Extensions.DependencyInjection;
using OpenTK.Graphics.OpenGL;
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
    private ViewableBezierObject _bezierObject;
    private byte _step;
    private float _scale = 1.0f;

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

        _bezierObject = new(ViewModel.PlaneView, Vector2D.Zero);

        var observable = Observable.FromEvent<TimeSpan>(
            handler => OpenTkControl.Render += handler,
            handler => OpenTkControl.Render -= handler);

        this.WhenActivated(disposables =>
        {
            observable.Subscribe(ts => _baseGraphic.Render(ts)).DisposeWith(disposables);

            this.WhenAnyValue(t => t._bezierObject.BezierWrapper.P3)
                .Subscribe(_ =>
                {
                    ViewModel.PlaneView.Plane.Points.Clear();

                    for (int i = 0; i < 20; i++)
                    {
                        var t = i / 20.0f;

                        ViewModel.PlaneView.Plane.Points.AddOrUpdate(_bezierObject.BezierWrapper.GenCurve(t));
                    }

                    ViewModel.PlaneView.Draw(_baseGraphic);
                });

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

            OpenTkControl.Events().MouseDown.Subscribe(args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                switch (_step)
                {
                    case 0:
                        _bezierObject.BezierWrapper.P0 = (x, y);
                        ViewModel.PlaneView.Plane.Points.AddOrUpdate((x, y));
                        _step++;
                        break;
                    case 1:
                        _bezierObject.BezierWrapper.P1 = (x, y);
                        ViewModel.PlaneView.Plane.Points.AddOrUpdate((x, y));
                        _step++;
                        break;
                    case 2:
                        _bezierObject.BezierWrapper.P2 = (x, y);
                        ViewModel.PlaneView.Plane.Points.AddOrUpdate((x, y));
                        _step++;
                        break;
                    case 3:
                        _bezierObject.BezierWrapper.P3 = (x, y);
                        ViewModel.PlaneView.Plane.Points.AddOrUpdate((x, y));
                        _step = 0;
                        break;
                }
            }).DisposeWith(disposables);

            OpenTkControl.Events().MouseMove.Subscribe(args =>
            {
                var point = args.GetPosition(OpenTkControl);

                var x = (float)(-1.0f + 2 * point.X / OpenTkControl.RenderSize.Width);
                var y = (float)(1.0f - 2 * point.Y / OpenTkControl.RenderSize.Height);

                MousePositionX.Text = x.ToString("G7", CultureInfo.InvariantCulture);
                MousePositionY.Text = y.ToString("G7", CultureInfo.InvariantCulture);

                ViewModel.PlaneView.Plane.Points.AddOrUpdate((x, y));
            }).DisposeWith(disposables);
        });
    }
}

#nullable restore