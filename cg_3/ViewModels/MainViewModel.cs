using cg_3.Models;
using cg_3.Source.Render;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_3.ViewModels;

public class MainViewModel : ReactiveObject, IViewable
{
    public PlaneView PlaneView { get; }

    public MainViewModel() => PlaneView = new();
    
    public void Draw(IBaseGraphic baseGraphic) => baseGraphic.DrawPoints(PlaneView.Plane.Points);
}