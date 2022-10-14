namespace cg_2.Source;

public struct Vector3
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    [JsonIgnore] public int Size => 3;

    public Vector3(float x, float y, float z) =>
        (X, Y, Z) = (x, y, z);

    public float Norm() => (float)Math.Sqrt(X * X + Y * Y + Z * Z);
    public Vector3 Normalize() => this / Norm();
    public Vector3 Cross(Vector3 other) =>
        new (
            Y * other.Z - Z * other.Y,
            Z * other.X - X * other.Z,
            X * other.Y - Y * other.X
        );

    public static Vector3 operator +(Vector3 a, Vector3 b) =>
        new (a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vector3 operator -(Vector3 a, Vector3 b) =>
        new (a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static float operator *(Vector3 a, Vector3 b) =>
        a.X * b.X + a.Y * b.Y + a.Z * b.Z;
    public static Vector3 operator *(Vector3 vector, float value) =>
        new (vector.X * value, vector.Y * value, vector.Z * value);
    public static Vector3 operator /(Vector3 vector, float value) =>
        new (vector.X / value, vector.Y / value, vector.Z / value);
}