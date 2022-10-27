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
        var width = (float)shaderProgram.Control.RenderSize.Width;
        var height = (float)shaderProgram.Control.RenderSize.Height;

        Projection = (camera.GetProjectionMatrix(),
            Projection.Name);
        View = (camera.GetViewMatrix(), View.Name);

        shaderProgram.SetUniform(View.Name, View.ViewMatrix);
        shaderProgram.SetUniform(Projection.Name, Projection.ProjectionMatrix);
        shaderProgram.SetUniform(Model.Name, Model.ModelMatrix);
    }
}

public class Lighting : IUniformContext
{
    public (Color4 Color, string Name) Color { get; }
    public (Vector3 LightColor, string Name) LightColor { get; }
    public (Vector3 LightPos, string Name) LightPos { get; }

    public Lighting((Color4 Color, string Name) color, (Vector3 LightColor, string Name) lightColor,
        (Vector3 LightPos, string Name) lightPos)
    {
        Color = color;
        LightColor = lightColor;
        LightPos = lightPos;
    }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(Color.Name, Color.Color.R,
            Color.Color.G, Color.Color.B);
        shaderProgram.SetUniform(LightColor.Name, LightColor.LightColor);
        shaderProgram.SetUniform(LightPos.Name, LightPos.LightPos);
    }
}