namespace cg_2.Source.Primitives;

public static class Primitives
{
    public static Vertex[] Cube(float side) => new Vertex[]
    {
        new(new(-side, -side, -side), new(0.0f, 0.0f, -1.0f)),
        new(new(side, -side, -side), new(0.0f, 0.0f, -1.0f)),
        new(new(side, side, -side), new(0.0f, 0.0f, -1.0f)),
        new(new(side, side, -side), new(0.0f, 0.0f, -1.0f)),
        new(new(-side, side, -side), new(0.0f, 0.0f, -1.0f)),
        new(new(-side, -side, -side), new(0.0f, 0.0f, -1.0f)),

        new(new(-side, -side, side), new(0.0f, 0.0f, 1.0f)),
        new(new(side, -side, side), new(0.0f, 0.0f, 1.0f)),
        new(new(side, side, side), new(0.0f, 0.0f, 1.0f)),
        new(new(side, side, side), new(0.0f, 0.0f, 1.0f)),
        new(new(-side, side, side), new(0.0f, 0.0f, 1.0f)),
        new(new(-side, -side, side), new(0.0f, 0.0f, 1.0f)),

        new(new(-side, side, side), new(-1.0f, 0.0f, 0.0f)),
        new(new(-side, side, -side), new(-1.0f, 0.0f, 0.0f)),
        new(new(-side, -side, -side), new(-1.0f, 0.0f, 0.0f)),
        new(new(-side, -side, -side), new(-1.0f, 0.0f, 0.0f)),
        new(new(-side, -side, side), new(-1.0f, 0.0f, 0.0f)),
        new(new(-side, side, side), new(-1.0f, 0.0f, 0.0f)),

        new(new(side, side, side), new(1.0f, 0.0f, 0.0f)),
        new(new(side, side, -side), new(1.0f, 0.0f, 0.0f)),
        new(new(side, -side, -side), new(1.0f, 0.0f, 0.0f)),
        new(new(side, -side, -side), new(1.0f, 0.0f, 0.0f)),
        new(new(side, -side, side), new(1.0f, 0.0f, 0.0f)),
        new(new(side, side, side), new(1.0f, 0.0f, 0.0f)),

        new(new(-side, -side, -side), new(0.0f, -1.0f, 0.0f)),
        new(new(side, -side, -side), new(0.0f, -1.0f, 0.0f)),
        new(new(side, -side, side), new(0.0f, -1.0f, 0.0f)),
        new(new(side, -side, side), new(0.0f, -1.0f, 0.0f)),
        new(new(-side, -side, side), new(0.0f, -1.0f, 0.0f)),
        new(new(-side, -side, -side), new(0.0f, -1.0f, 0.0f)),

        new(new(-side, side, -side), new(0.0f, 1.0f, 0.0f)),
        new(new(side, side, -side), new(0.0f, 1.0f, 0.0f)),
        new(new(side, side, side), new(0.0f, 1.0f, 0.0f)),
        new(new(side, side, side), new(0.0f, 1.0f, 0.0f)),
        new(new(-side, side, side), new(0.0f, 1.0f, 0.0f)),
        new(new(-side, side, -side), new(0.0f, 1.0f, 0.0f)),
    };
}