using System.Linq;

namespace cg_2.Source.Polygon;

public class PolygonSection
{
    public List<vec3> Vertices { get; }

    public PolygonSection(vec3[] vertices) => Vertices = vertices.ToList();

    public vec3 MassCenter()
    {
        float x = 0f, y = 0f, z = 0f;

        foreach (var v in Vertices)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }

        x = x / Vertices.Count;
        y = y / Vertices.Count;
        z = z / Vertices.Count;

        return new vec3(x, y, z);
    }

    public static PolygonSection ReadJson(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                throw new Exception("File doesn't exist\n");
            }

            using var sr = new StreamReader(path);
            return JsonConvert.DeserializeObject<PolygonSection>(sr.ReadToEnd()) ??
                   throw new NullReferenceException("Fill in the file correctly\n");
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: " + e.Message);
            throw;
        }
    }

    public static void WriteJson(PolygonSection section, string path)
    {
        using var sw = new StreamWriter(path);
        
        sw.Write(JsonConvert.SerializeObject(section));
    }
}