namespace cg_2.Source.Polygon;

public class PolygonSection : ICloneable
{
    public List<vec3> Vertices { get; }
    public int VertexCount => Vertices.Count;

    public PolygonSection() => Vertices = new List<vec3>();

    public PolygonSection(IEnumerable<vec3> vertices) => Vertices = vertices.ToList();

    public static PolygonSection ReadJson(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist");
            }

            using var sr = new StreamReader(path);
            return JsonConvert.DeserializeObject<PolygonSection>(sr.ReadToEnd()) ??
                   throw new NullReferenceException("Fill in the file correctly");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception:{ex.Message}");
            throw;
        }
    }

    public static void WriteJson(PolygonSection section, string path)
    {
        using var sw = new StreamWriter(path);
        sw.WriteLine(JsonConvert.SerializeObject(section));
    }

    public object Clone() => new PolygonSection(Vertices);
}