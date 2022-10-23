namespace cg_2.Source.Render;

public abstract class BaseInstance : IRenderable
{
    public ShaderProgramWrapper ShaderProgram { get; }
    public VertexBufferArray? Vao { get; protected set; }
    public VertexBufferWrapper? Vbo { get; protected set; }
    public float[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public MaterialColor? MaterialColor { get; }

    protected BaseInstance(ShaderProgramWrapper shaderProgram, float[] vertices, IUniformContext[] uniformContext,
        MaterialColor? materialColor = null)
    {
        ShaderProgram = shaderProgram;
        Vertices = vertices;
        UniformContext = uniformContext;
        MaterialColor = materialColor ?? MaterialColor.Standart;
    }

    public virtual void Initialize(VertexBufferArray vao, VertexBufferWrapper vbo)
    {
        Vao = vao;
        Vbo = vbo;

        var glContext = ShaderProgram.CurrentOpenGLContext;

        vao.Create(glContext);
        vbo.Create(glContext);

        vao.Bind(glContext);
        vbo.Bind(glContext);

        vbo.SetData(glContext, 0, Vertices, false, 3, 8, 0);
        vbo.SetData(glContext, 1, Vertices, false, 3, 8, 3);
        vbo.SetData(glContext, 2, Vertices, false, 2, 8, 6);

        vao.Unbind(glContext);
    }

    public void UpdateUniform(MainCamera camera)
        => Array.ForEach(UniformContext, item => item.Update(ShaderProgram, camera));
}

public class CompleteInstance : BaseInstance
{
    public CompleteInstance(ShaderProgramWrapper shaderProgram, float[] vertices, IUniformContext[] uniformContext,
        MaterialColor? materialColor = null) : base(
        shaderProgram, vertices, uniformContext, materialColor)
    {
    }
}

public class SolidInstance : BaseInstance
{
    public SolidInstance(ShaderProgramWrapper shaderProgram, float[] vertices, IUniformContext[] uniformContext,
        MaterialColor? materialColor = null) : base(
        shaderProgram, vertices, uniformContext, materialColor)
    {
    }

    public override void Initialize(VertexBufferArray vao, VertexBufferWrapper vbo)
    {
        Vao = vao;
        Vbo = vbo;

        var glContext = ShaderProgram.CurrentOpenGLContext;

        vao.Create(glContext);
        vbo.Create(glContext);

        vao.Bind(glContext);
        vbo.Bind(glContext);

        vbo.SetData(glContext, 0, Vertices, false, 3, 6, 0);
        vbo.SetData(glContext, 1, Vertices, false, 3, 6, 3);

        vao.Unbind(glContext);
    }
}

public class StandartInstance : BaseInstance
{
    public StandartInstance(ShaderProgramWrapper shaderProgram, float[] vertices, IUniformContext[] uniformContext,
        MaterialColor? materialColor = null) : base(
        shaderProgram, vertices, uniformContext, materialColor)
    {
    }

    public override void Initialize(VertexBufferArray vao, VertexBufferWrapper vbo)
    {
        Vao = vao;
        Vbo = vbo;

        var glContext = ShaderProgram.CurrentOpenGLContext;

        vao.Create(glContext);
        vbo.Create(glContext);

        vao.Bind(glContext);
        vbo.Bind(glContext);

        vbo.SetData(glContext, 0, Vertices, false, 3, 3, 0);

        vao.Unbind(glContext);
    }
}