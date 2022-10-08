namespace cg_2.Source;

public readonly record struct Vector3(float X, float Y, float Z)
{
    public static readonly Vector3 Zero = default;
    public int Size => 3;
    public float Norm { get; } = (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public static float Distance(Vector3 a, Vector3 b) => (a - b).Norm;

    public float Distance(Vector3 b) => Distance(this, b);

    public Vector3 Normalize() => this / Norm;

    public Vector3 Cross(Vector3 vector) =>
        new(
            Y * vector.Z - Z * vector.Y,
            Z * vector.X - X * vector.Z,
            X * vector.Y - Y * vector.X
        );

    public static Vector3 operator +(Vector3 a, Vector3 b)
        => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

    public static Vector3 operator -(Vector3 a, Vector3 b)
        => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

    public static float operator *(Vector3 a, Vector3 b)
        => a.X * b.X + a.Y * b.Y + a.Z * b.Z;

    public static Vector3 operator *(Vector3 vector, float value)
        => new(vector.X * value, vector.Y * value, vector.Z * value);

    public static Vector3 operator /(Vector3 vector, float value)
        => new(vector.X / value, vector.Y / value, vector.Z / value);
}