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

public readonly record struct Attenuation(float Constant, float Linear, float Quadratic)
{
    public static Attenuation Range3250 => new(1.0f, 0.0014f, 0.000007f);

    public static Attenuation Range600 => new(1.0f, 0.007f, 0.002f);

    public static Attenuation Range200 => new(1.0f, 0.0022f, 0.0019f);

    public static Attenuation Range160 => new(1.0f, 0.027f, 0.0028f);

    public static Attenuation Range100 => new(1.0f, 0.0045f, 0.0075f);

    public static Attenuation Range65 => new(1.0f, 0.07f, 0.017f);

    public static Attenuation Range50 => new(1.0f, 0.09f, 0.032f);

    public static Attenuation Range32 => new(1.0f, 0.14f, 0.07f);

    public static Attenuation Range20 => new(1.0f, 0.22f, 0.2f);

    public static Attenuation Range13 => new(1.0f, 0.35f, 0.44f);

    public static Attenuation Range7 => new(1.0f, 0.7f, 1.8f);
}

public readonly record struct Spot(float CutOff, float OuterCutOff);

public class Lighting : IUniformContext
{
    public (Vector3 Value, string Name) AmbientContext { get; }
    public (Vector3 Value, string Name) DiffuseContext { get; }
    public (Vector3 Value, string Name) SpecularContext { get; }
    public (Vector3 Value, string Name) LightPosContext { get; set; }
    public (Vector3 Value, string Name) LightDirContext { get; set; }
    public (Attenuation Value, string NameConst, string NameLin, string NameQuad) AttenuationContext { get; }
    public (Spot Value, string NameCut, string NameOuterCut) SpotContext { get; }
    public string ViewPosName { get; }

    public Lighting((Vector3 Value, string Name) ambientContext,
        (Vector3 Value, string Name) diffuseContext,
        (Vector3 Value, string Name) specularContext,
        (Vector3 Value, string Name) lightPosContext,
        (Vector3 Value, string Name) lightDirContext,
        (Attenuation Value, string NameConst, string NameLin, string NameQuad) attenuationContext,
        (Spot Value, string NameCut, string NameOuterCut) spotContext,
        string viewPosName)
    {
        AmbientContext = ambientContext;
        DiffuseContext = diffuseContext;
        SpecularContext = specularContext;
        LightPosContext = lightPosContext;
        LightDirContext = lightDirContext;
        AttenuationContext = attenuationContext;
        SpotContext = spotContext;
        ViewPosName = viewPosName;
    }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(AmbientContext.Name, AmbientContext.Value);
        shaderProgram.SetUniform(DiffuseContext.Name, DiffuseContext.Value);
        shaderProgram.SetUniform(LightPosContext.Name, LightPosContext.Value);
        shaderProgram.SetUniform(LightDirContext.Name, LightDirContext.Value);
        shaderProgram.SetUniform(AttenuationContext.NameConst, AttenuationContext.Value.Constant);
        shaderProgram.SetUniform(AttenuationContext.NameLin, AttenuationContext.Value.Linear);
        shaderProgram.SetUniform(AttenuationContext.NameQuad, AttenuationContext.Value.Quadratic);
        shaderProgram.SetUniform(SpotContext.NameCut, SpotContext.Value.CutOff);
        shaderProgram.SetUniform(SpotContext.NameOuterCut, SpotContext.Value.OuterCutOff);
        shaderProgram.SetUniform(ViewPosName, camera.Position);
    }

    // public static Lighting DarkenLight((Vector3 Value, string Name) position, string viewPosName)
    //     => new(
    //         (new(0.2f), "light.ambient"),
    //         (new(0.5f), "light.diffuse"), (new(1.0f), "light.specular"),
    //         position, (new(1.0f, 0.0f, 0.0f), "light.constant", "light.linear", "light.quadratic"), viewPosName);

    public static Lighting BrightLight((Vector3 Value, string Name) position, (Vector3 Value, string Name) direction,
        string viewPosName)
        => new(
            (new(1.0f), "light.ambient"),
            (new(1.0f), "light.diffuse"),
            (new(1.0f), "light.specular"),
            position, direction, (new(1.0f, 0.0f, 0.0f), "light.constant", "light.linear", "light.quadratic"),
            (new(MathF.Cos(MathHelper.DegreesToRadians(12.5f)), MathF.Cos(MathHelper.DegreesToRadians(17.5f))),
                "light.cutOff", "light.outerCutOff"),
            viewPosName);

    // public static Lighting BrightLightAttenuation600((Vector3 Value, string Name) position, string viewPosName)
    //     => new(
    //         (new(1.0f), "light.ambient"),
    //         (new(1.0f), "light.diffuse"),
    //         (new(1.0f), "light.specular"),
    //         position, (Attenuation.Range600, "light.constant", "light.linear", "light.quadratic"), viewPosName);
    //
    // public static Lighting BrightLightAttenuation200((Vector3 Value, string Name) position, string viewPosName)
    //     => new(
    //         (new(1.0f), "light.ambient"),
    //         (new(1.0f), "light.diffuse"),
    //         (new(1.0f), "light.specular"),
    //         position, (Attenuation.Range600, "light.constant", "light.linear", "light.quadratic"), viewPosName);
    //
    // public static Lighting BrightLightAttenuation50((Vector3 Value, string Name) position, string viewPosName)
    //     => new(
    //         (new(1.0f), "light.ambient"),
    //         (new(1.0f), "light.diffuse"),
    //         (new(1.0f), "light.specular"),
    //         position, (Attenuation.Range50, "light.constant", "light.linear", "light.quadratic"), viewPosName);
    //
    // public static Lighting BrightLightAttenuation7((Vector3 Value, string Name) position, string viewPosName)
    //     => new(
    //         (new(1.0f), "light.ambient"),
    //         (new(1.0f), "light.diffuse"),
    //         (new(1.0f), "light.specular"),
    //         position, (Attenuation.Range7, "light.constant", "light.linear", "light.quadratic"), viewPosName);
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