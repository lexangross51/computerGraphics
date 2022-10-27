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
        (Matrix4 ProjectioMatrix, string name) projection, (Matrix4 ModelMatrix, string Name) model)
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
    public (Color4 Color, string Name) ColorContext { get; }
    public (Vector3 LightColor, string Name) LightColorContext { get; }
    public (Vector3 LightPos, string Name) LightPosContext { get; }
    public string ViewPosName { get; }

    public Lighting((Color4 Color, string Name) colorContext, (Vector3 LightColor, string Name) lightColorContext,
        (Vector3 LightPos, string Name) lightPosContext, string viewPosName)
    {
        ColorContext = colorContext;
        LightColorContext = lightColorContext;
        LightPosContext = lightPosContext;
        ViewPosName = viewPosName;
    }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(ColorContext.Name, new Vector3(ColorContext.Color.A, ColorContext.Color.G, ColorContext.Color.B));
        shaderProgram.SetUniform(LightColorContext.Name, LightColorContext.LightColor);
        shaderProgram.SetUniform(LightPosContext.Name, LightPosContext.LightPos);
        shaderProgram.SetUniform(ViewPosName, camera.Position);
    }
}