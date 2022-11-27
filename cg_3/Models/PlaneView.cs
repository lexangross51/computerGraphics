using System.Collections.ObjectModel;
using System.Reactive;
using cg_3.Source.Vectors;
using cg_3.ViewModels;
using DynamicData;
using ReactiveUI;

namespace cg_3.Models;

public class PlaneView
{
    private readonly ReadOnlyObservableCollection<BezierWrapper> _wrappers;
    private SourceCache<BezierWrapper, BezierWrapper> _wrappersAsSourceCache = new(w => w);
    public Plane Plane { get; }
    public ReadOnlyObservableCollection<BezierWrapper> Wrappers => _wrappers;
    public ReactiveCommand<BezierWrapper, Unit> AddWrapper { get; }

    public PlaneView()
    {
        Plane = new();
        _wrappersAsSourceCache.Connect().Bind(out _wrappers).Subscribe();
        AddWrapper = ReactiveCommand.Create<BezierWrapper>(wrapper => _wrappersAsSourceCache.AddOrUpdate(wrapper));
    }
}

public class Plane
{
    public List<Vector2D> Points { get; set; }

    public Plane() => Points = new();
}