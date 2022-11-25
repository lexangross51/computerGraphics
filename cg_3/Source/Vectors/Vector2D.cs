using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cg_3.Source.Vectors;

public class Vector2DJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType) => typeof(Vector2D) == objectType;

    public override object ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.StartArray)
        {
            var array = JArray.Load(reader);
            if (array.Count == 2) return new Vector2D(array[0].Value<float>(), array[1].Value<float>());
            throw new FormatException($"Wrong vector length({array.Count})!");
        }

        if (Vector2D.TryParse((string?)reader.Value ?? "", out var vec)) return vec;
        throw new FormatException($"Can't parse({(string?)reader.Value}) as Vector2D!");
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        value ??= new Vector2D();
        var vec = (Vector2D)value;
        writer.WriteRawValue($"[{vec.X}, {vec.Y}]");
        // [[0, 0],[0, 0]] // runtime exception
        // [[0, 0][0, 0]]
    }
}

[JsonConverter(typeof(Vector2DJsonConverter))]
public readonly record struct Vector2D(float X, float Y)
{
    public float Norm { get; } = (float)Math.Sqrt(X * X + Y * Y);
    public static int Size => 2;
    public static readonly Vector2D Zero = default;

    public static float Distance(Vector2D a, Vector2D b) => (a - b).Norm;

    public float Distance(Vector2D b) => Distance(this, b);

    public Vector2D Normalize() => this / Norm;

    public override string ToString()
        => $"Vec2({X}, {Y})";

    public static bool TryParse(string line, out Vector2D vector)
    {
        var words = line.Split(new[] { ' ', ',', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
        if (words.Length != 3 || !float.TryParse(words[1], out var x) || !float.TryParse(words[2], out var y))
        {
            vector = Zero;
            return false;
        }

        vector = new(x, y);
        return true;
    }

    public static Vector2D Parse(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            throw new ArgumentException("Is null or white space", nameof(line));
        }

        if (!TryParse(line, out var vector))
        {
            throw new FormatException("Can't parse Vector2D");
        }

        return vector;
    }

    #region Static Operators

    public static Vector2D operator +(Vector2D a, Vector2D b)
        => new(a.X + b.X, a.Y + b.Y);

    public static Vector2D operator -(Vector2D a, Vector2D b)
        => new(a.X - b.X, a.Y - b.Y);

    public static float operator *(Vector2D a, Vector2D b)
        => a.X * b.X + a.Y * b.Y;

    public static Vector2D operator *(Vector2D vector, float value)
        => new(vector.X * value, vector.Y * value);

    public static Vector2D operator *(float value, Vector2D vector)
        => new(value * vector.X, value * vector.Y);

    public static Vector2D operator /(Vector2D vector, float value)
        => new(vector.X / value, vector.Y / value);

    public static Vector2D operator -(Vector2D vector)
        => new(-vector.X, -vector.Y);

    #endregion
}