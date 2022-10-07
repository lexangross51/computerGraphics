namespace cg_2.Source;

public class TransformationMatrix
{
    private readonly float[,] _storage;
    public int Size => 4;

    public TransformationMatrix()
    {
        _storage = new float[Size, Size];
    }

    public float this[int i, int j]
    {
        get => _storage[i, j];
        set => _storage[i, j] = value;
    }

    public static Vector3 operator *(TransformationMatrix matrix, Vector3 vector)
    {
        var x = matrix[0, 0] * vector.X + matrix[0, 1] * vector.Y + matrix[0, 2] * vector.Z + matrix[0, 3];
        var y = matrix[1, 0] * vector.X + matrix[1, 1] * vector.Y + matrix[1, 2] * vector.Z + matrix[1, 3];
        var z = matrix[2, 0] * vector.X + matrix[2, 1] * vector.Y + matrix[2, 2] * vector.Z + matrix[2, 3];

        return new(x, y, z);
    }
}