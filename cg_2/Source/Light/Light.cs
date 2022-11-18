namespace cg_2.Source.Light;

public enum LightType
{
    Directional,
    Point,
    PointWithAttenuation,
    Spot
}

public class Light
{
    public vec3 Ambient { get; set; }
    public vec3 Diffuse { get; set; }
    public vec3 Specular { get; set; }
    public float Constant { get; set; }
    public float Linear { get; set; }
    public float Quadratic { get; set; }
    public float CutOff { get; set; }
    public float OuterCutOff { get; set; }
    public LightType LightType { get; set; }

    public static Light DirectionalLight => new()
    {
        Ambient = new(1.0f),
        Diffuse = new(1.0f),
        Specular = new(1.0f),
        Constant = 1.0f,
        Linear = 0.0f,
        Quadratic = 0.0f,
        CutOff = 1.0f,
        OuterCutOff = 1.0f,
        LightType = LightType.Directional
    };

    public static Light PointLight => new()
    {
        Ambient = new(0.7f),
        Diffuse = new(0.7f),
        Specular = new(0.7f),
        Constant = 1.0f,
        Linear = 0.0f,
        Quadratic = 0.0f,
        CutOff = 1.0f,
        OuterCutOff = 1.0f,
        LightType = LightType.Point
    };

    public static Light PointLightWithAttenuation => new()
    {
        Ambient = new(1.0f),
        Diffuse = new(1.0f),
        Specular = new(1.0f),
        Constant = 1.0f,
        Linear = 0.07f,
        Quadratic = 0.0017f,
        CutOff = 1.0f,
        OuterCutOff = 1.0f,
        LightType = LightType.PointWithAttenuation
    };

    public static Light SpotLight => new()
    {
        Ambient = new(1.0f),
        Diffuse = new(1.0f),
        Specular = new(1.0f),
        Constant = 1.0f,
        Linear = 0.0f,
        Quadratic = 0.0f,
        CutOff = MathF.Cos(glm.radians(12.5f)),
        OuterCutOff = MathF.Cos(glm.radians(17.5f)),
        LightType = LightType.Spot
    };
}