namespace cg_3.ViewModels;

public class BezierWrapper : ReactiveObject
{
    public BezierObject Curve { get; }

    public Vector2D P0
    {
        get => Curve[0];
        set
        {
            if (Curve[0] == value) return;
            Curve[0] = value;
            this.RaisePropertyChanged();
        }
    }
    
    public Vector2D P1
    {
        get => Curve[1];
        set
        {
            if (Curve[1] == value) return;
            Curve[1] = value;
            this.RaisePropertyChanged();
        }
    }
    
    public Vector2D P2
    {
        get => Curve[2];
        set
        {
            if (Curve[2] == value) return;
            Curve[2] = value;
            this.RaisePropertyChanged();
        }
    }
    
    public Vector2D P3
    {
        get => Curve[3];
        set
        {
            if (Curve[3] == value) return;
            Curve[3] = value;
            this.RaisePropertyChanged();
        }
    }

    public BezierWrapper(BezierObject curve) => Curve = curve;
}