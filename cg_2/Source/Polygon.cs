namespace cg_2.Source;

public class Polygon
{
    [JsonProperty("Points", Required = Required.Always)]
    private readonly List<Vector3> _points;

    public Polygon() => _points = new();

    public Vector3 this[int idx] => _points[idx];

    public Vector3 MassCenter()
    {
        var x = 0.0f;
        var y = 0.0f;
        var z = 0.0f;

        foreach (var p in _points)
        {
            x += p.X;
            y += p.Y;
            z += p.Z;
        }

        x /= _points.Count;
        y /= _points.Count;
        z /= _points.Count;

        return new(x, y, z);
    }

    // TODO
    public Polygon Translate(Vector3 direction)
    {
        return new();
    }

    // TODO
    public void Scale(Vector3 pivot, float scaling)
    {

    }
}