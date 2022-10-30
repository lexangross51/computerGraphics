namespace cg_2.Source.Material;

public readonly record struct Material
{
    public vec3 Ambient { get; }
    public vec3 Diffuse { get; }
    public vec3 Specular { get; }
    public float Shininess { get; }

    public Material(vec3 ambient, vec3 diffuse, vec3 specular, float shininess)
    {
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
        Shininess = shininess;
    }

    public static Material GoldMaterial
        => new(new(0.24725f, 0.1995f, 0.0745f),
            new(0.75164f, 0.60648f, 0.22648f),
            new(0.628281f, 0.555802f, 0.366065f),
            0.4f);

    public static Material PearlMaterial
        => new(new(0.25f, 0.20725f, 0.20725f),
            new(1.0f, 0.829f, 0.829f),
            new(0.0296648f),
            0.088f);

    public static Material BlackPlasticMaterial
        => new(new(0.0f),
            new(0.01f),
            new(0.5f),
            0.25f);
}