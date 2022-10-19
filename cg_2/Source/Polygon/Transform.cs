namespace cg_2.Source.Polygon;

public class Transform
{
    public vec3 Trajectory { get; }
    public float Scale { get; }
    public float Angle { get; }

    public Transform(vec3 trajectory, float scale, float angle)
        => (Trajectory, Scale, Angle) = (trajectory, scale, angle);

    public static IEnumerable<Transform> ReadJson(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist\n");
            }

            using var sr = new StreamReader(path);
            return JsonConvert.DeserializeObject<Transform[]>(sr.ReadToEnd()) ??
                   throw new NullReferenceException("Fill in the file correctly\n");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: " + e.Message);
            throw;
        }
    }

    public static void WriteJson(Transform[] transformations, string path)
    {
        using var sw = new StreamWriter(path);
        sw.Write(JsonConvert.SerializeObject(transformations));
    }
}