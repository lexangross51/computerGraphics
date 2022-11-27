using cg_3.Source.Camera;
using cg_3.Source.Wrappers;
using OpenTK.Mathematics;

namespace cg_3.Source.Render;

public interface IUniformContext
{
    public void Update(ShaderProgram shaderProgram, MainCamera camera);
}

public class Transformation : IUniformContext
{
    public required (Matrix4 ViewMatrix, string Name) View { get; set; }
    public required (Matrix4 ProjectionMatrix, string Name) Projection { get; set; }
    public required (Matrix4 ModelMatrix, string Name) Model { get; set; }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        Projection = (camera.GetProjectionMatrix(), Projection.Name);
        View = (camera.GetViewMatrix(), View.Name);

        shaderProgram.SetUniform(View.Name, View.ViewMatrix);
        shaderProgram.SetUniform(Projection.Name, Projection.ProjectionMatrix);
        shaderProgram.SetUniform(Model.Name, Model.ModelMatrix);
    }
}