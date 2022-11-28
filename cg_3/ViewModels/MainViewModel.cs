using cg_3.Models;
using cg_3.Source.Render;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class MainViewModel : ReactiveObject
{
    public PlaneView PlaneView { get; }

    [Reactive] public BezierWrapper BezierWrapper { get; set; }

    public MainViewModel()
    {
        PlaneView = new();
        PlaneView.Wrappers.Connect().OnItemAdded(wrapper => BezierWrapper = wrapper).Subscribe();
    }
}