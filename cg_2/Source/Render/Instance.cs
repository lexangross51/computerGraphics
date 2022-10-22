namespace cg_2.Source.Render;

public class Instance : IRenderable
{
    public ShaderProgramWrapper ShaderProgram { get; }
    public float[] Vertices { get; }
    public MaterialColor? MaterialColor { get; }

    public Instance(ShaderProgramWrapper shaderProgram, float[] vertices, MaterialColor? materialColor = null)
    {
        ShaderProgram = shaderProgram;
        Vertices = vertices;
        MaterialColor = materialColor ?? MaterialColor.Standart;

        GenerateBuffers();
    }

    private void GenerateBuffers()
    {
        VertexBufferArray vao = new();
        VertexBufferWrapper vbo = new(new());

        var glContext = ShaderProgram.CurrentOpenGLContext;

        vao.Create(glContext);
        vbo.Create(glContext);

        vao.Bind(glContext);
        vbo.Bind(glContext);

        // vbo.SetData(glContext, 0, Vertices, false, 3, 8, 0);
        // vbo.SetData(glContext, 1, Vertices, false, 3, 8, 3);
        // vbo.SetData(glContext, 2, Vertices, false, 2, 8, 6);

        vbo.SetData(glContext, 0, Vertices, false, 3, 6, 0);
        vbo.SetData(glContext, 1, Vertices, false, 3, 6, 3);

        vao.Unbind(glContext);
    }
}