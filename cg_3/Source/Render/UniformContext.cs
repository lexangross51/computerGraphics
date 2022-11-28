using cg_3.Source.Camera;
using cg_3.Source.Wrappers;
using OpenTK.Mathematics;

namespace cg_3.Source.Render;

public interface IUniformContext
{
    void Update(ShaderProgram shaderProgram, MainCamera camera);
}

public class Transformation : IUniformContext
{
    public required (Matrix4 ViewMatrix, string Name) View { get; set; }
    public required (Matrix4 ProjectionMatrix, string Name) Projection { get; set; }
    public required (Matrix4 ModelMatrix, string Name) Model { get; set; }

    public void Update(ShaderProgram shaderProgram, MainCamera camera)
    {
        shaderProgram.SetUniform(View.Name, View.ViewMatrix);
        shaderProgram.SetUniform(Projection.Name, Projection.ProjectionMatrix);
        shaderProgram.SetUniform(Model.Name, Model.ModelMatrix);
    }

    public static IUniformContext DefaultUniformTransformationContext => new Transformation
    {
        View = (Matrix4.Identity, "view"),
        Projection = (Matrix4.Identity, "projection"),
        Model = (Matrix4.Identity, "model")
    };
}