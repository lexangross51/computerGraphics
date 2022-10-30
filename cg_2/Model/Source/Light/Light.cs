namespace cg_2.Model.Source.Light;

public enum LightType
{
    Directional,
    Point,
    PointWithAttenuation,
    Spot
}

public struct Light
{
    public vec3 Ambient { get; init; }
    public vec3 Diffuse { get; init; }
    public vec3 Specular { get; init; }
    public float Constant { get; init; }
    public float Linear { get; init; }
    public float Quadratic { get; init; }
    public float CutOff { get; init; }
    public float OuterCutOff { get; init; }
    public LightType LightType { get; init; }

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
        Ambient = new(1.0f),
        Diffuse = new(1.0f),
        Specular = new(1.0f),
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