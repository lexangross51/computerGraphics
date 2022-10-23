namespace cg_2.Source.UniformsContext;

public interface IUniformContext
{
    public void Update(ShaderProgramWrapper shaderProgram, MainCamera camera);
}

public class Transformation : IUniformContext
{
    public (mat4 ViewMatrix, string Name) View { get; set; }
    public (mat4 ProjectionMatrix, string Name) Projection { get; set; }
    public (mat4 ModelMatrix, string Name) Model { get; set; }

    public Transformation((mat4 ViewMatrix, string Name) view,
        (mat4 ProjectioMatrix, string name) projection, (mat4 ModelMatrix, string Name) model)
    {
        View = view;
        Projection = projection;
        Model = model;
    }

    public void Update(ShaderProgramWrapper shaderProgram, MainCamera camera)
    {
        var width = (float)shaderProgram.CurrentOpenGLContext.RenderContextProvider.Width;
        var height = (float)shaderProgram
            .CurrentOpenGLContext.RenderContextProvider.Height;

        Projection = (camera.CameraMode == CameraMode.Perspective
                ? glm.perspective(45.0f,
                    width / height, 0.1f, 100.0f)
                : glm.ortho(-width / 50.0f, width / 50.0f, -height / 50.0f, height / 50.0f, 0.1f, 100.0f),
            Projection.Name);
        View = (glm.lookAt(camera.Position, camera.Position + camera.Front, camera.Up), View.Name);

        shaderProgram.SetUniform(View.Name, View.ViewMatrix);
        shaderProgram.SetUniform(Projection.Name, Projection.ProjectionMatrix);
        shaderProgram.SetUniform(Model.Name, Model.ModelMatrix);
    }
}

public class Lighting : IUniformContext
{
    public (MaterialColor Color, string Name) MaterialColor { get; }
    public (vec3 LightColor, string Name) LightColor { get; }
    public (vec3 LightPos, string Name) LightPos { get; }

    public Lighting((MaterialColor Color, string Name) materialColor, (vec3 LightColor, string Name) lightColor,
        (vec3 LightPos, string Name) lightPos)
    {
        MaterialColor = materialColor;
        LightColor = lightColor;
        LightPos = lightPos;
    }

    public void Update(ShaderProgramWrapper shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(MaterialColor.Name, MaterialColor.Color.Color.R.ToFloat(),
            MaterialColor.Color.Color.G.ToFloat(), MaterialColor.Color.Color.B.ToFloat());
        shaderProgram.SetUniform(LightColor.Name, 1.0f, 1.0f, 1.0f);
        shaderProgram.SetUniform(LightPos.Name, LightPos.LightPos.x, LightPos.LightPos.y, LightPos.LightPos.z);
    }
}