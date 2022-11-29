using System.Reactive;
using System.Reactive.Linq;
using cg_3.Models;
using cg_3.Source.Render;
using cg_3.Views;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class PlaneViewModel : ReactiveObject
{
    private readonly ObservableAsPropertyHelper<bool> _isDrawingMode;
    private readonly ObservableAsPropertyHelper<bool> _isSelectSegmentMode;
    public ReactiveCommand<Unit, bool> SetDrawingMode { get; }
    public ReactiveCommand<Unit, bool> SetSelectSegmentMode { get; }
    public bool IsDrawingMode => _isDrawingMode.Value;
    public bool IsSelectSegmentMode => _isSelectSegmentMode.Value;
    [Reactive] public BezierWrapper? BezierWrapper { get; set; }

    public PlaneViewModel()
    {
        SetDrawingMode = ReactiveCommand.CreateFromObservable<Unit, bool>(_ => Observable.Return(!IsDrawingMode));
        SetDrawingMode.ToProperty(this, t => t.IsDrawingMode, out _isDrawingMode);
        SetSelectSegmentMode = ReactiveCommand.CreateFromObservable<Unit, bool>(_ => Observable.Return(!IsSelectSegmentMode));
        SetSelectSegmentMode.ToProperty(this, t => t.IsSelectSegmentMode, out _isSelectSegmentMode);
    }
}