namespace cg_2.Source.UniformsContext;

public interface IUniformContext
{
    public void Update(ShaderProgram shaderProgram, MainCamera camera);
}

public class Transformation : IUniformContext
{
    public (Matrix4 ViewMatrix, string Name) View { get; set; }
    public (Matrix4 ProjectionMatrix, string Name) Projection { get; set; }
    public (Matrix4 ModelMatrix, string Name) Model { get; set; }

    public Transformation((Matrix4 ViewMatrix, string Name) view,
        (Matrix4 ProjectioMatrix, string name) projection,
        (Matrix4 ModelMatrix, string Name) model)
    {
        View = view;
        Projection = projection;
        Model = model;
    }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        Projection = (camera.GetProjectionMatrix(), Projection.Name);
        View = (camera.GetViewMatrix(), View.Name);

        shaderProgram.SetUniform(View.Name, View.ViewMatrix);
        shaderProgram.SetUniform(Projection.Name, Projection.ProjectionMatrix);
        shaderProgram.SetUniform(Model.Name, Model.ModelMatrix);
    }
}

public class Lighting : IUniformContext
{
    public (Vector3 Value, string Name) AmbientContext { get; }
    public (Vector3 Value, string Name) DiffuseContext { get; }
    public (Vector3 Value, string Name) SpecularContext { get; }
    public (Vector3 Value, string Name) LightPosContext { get; }
    public string ViewPosName { get; }

    public Lighting((Vector3 Value, string Name) ambientContext,
        (Vector3 Value, string Name) diffuseContext,
        (Vector3 Value, string Name) specularContext,
        (Vector3 Value, string Name) lightPosContext,
        string viewPosName)
    {
        AmbientContext = ambientContext;
        DiffuseContext = diffuseContext;
        SpecularContext = specularContext;
        LightPosContext = lightPosContext;
        ViewPosName = viewPosName;
    }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(AmbientContext.Name, AmbientContext.Value);
        shaderProgram.SetUniform(DiffuseContext.Name, DiffuseContext.Value);
        shaderProgram.SetUniform(LightPosContext.Name, LightPosContext.Value);
        shaderProgram.SetUniform(ViewPosName, camera.Position);
    }

    public static Lighting DarkenLight((Vector3 Value, string Name) position, string viewPosName)
        => new(
            (new(0.2f), "light.ambient"),
            (new(0.5f), "light.diffuse"), (new(1.0f), "light.specular"), position, viewPosName);

    public static Lighting BrightLight((Vector3 Value, string Name) position, string viewPosName)
        => new(
            (new(1.0f), "light.ambient"),
            (new(1.0f), "light.diffuse"), (new(1.0f), "light.specular"), position, viewPosName);
}

public readonly record struct Material((Vector3 Value, string Name) AmbientContext,
    (Vector3 Value, string Name) DiffuseContext,
    (Vector3 Value, string Name) SpecularContext,
    (float Value, string Name) ShininessContext) : IUniformContext
{
    public static Material StandartMaterial
        => new(((1.0f, 0.5f, 0.31f), "material.ambient"),
            ((1.0f, 0.5f, 0.31f), "material.diffuse"),
            (new(0.5f), "material.specular"),
            (32.0f, "material.shininess"));

    // need a bright light to match
    public static Material EmeraldMaterial
        => new(((0.0215f, 0.1745f, 0.0215f), "material.ambient"),
            ((0.07568f, 0.61424f, 0.07568f), "material.diffuse"),
            ((0.633f, 0.727811f, 0.633f), "material.specular"),
            (0.6f, "material.shininess"));

    public static Material PearlMaterial
        => new(((0.25f, 0.20725f, 0.20725f), "material.ambient"),
            ((1.0f, 0.829f, 0.829f), "material.diffuse"),
            (new(0.0296648f), "material.specular"),
            (0.088f, "material.shininess"));

    public static Material RedPlasticMaterial
        => new((new(0.0f), "material.ambient"),
            ((0.5f, 0.0f, 0.0f), "material.diffuse"),
            ((0.7f, 0.6f, 0.6f), "material.specular"),
            (0.25f, "material.shininess"));

    public static Material GoldMaterial
        => new((new(0.24725f, 0.1995f, 0.0745f), "material.ambient"),
            ((0.75164f, 0.60648f, 0.22648f), "material.diffuse"),
            (new(0.628281f, 0.555802f, 0.366065f), "material.specular"),
            (0.4f, "material.shininess"));


    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(AmbientContext.Name, AmbientContext.Value);
        shaderProgram.SetUniform(DiffuseContext.Name, DiffuseContext.Value);
        shaderProgram.SetUniform(SpecularContext.Name, SpecularContext.Value);
        shaderProgram.SetUniform(ShininessContext.Name, ShininessContext.Value);
    }
}