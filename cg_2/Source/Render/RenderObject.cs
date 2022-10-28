namespace cg_2.Source.Render;

public class RenderObject : IRenderable
{
    public ShaderProgram ShaderProgram { get; }
    public VertexArrayObject? Vao { get; private set; }
    public Vertex[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public PrimitiveType PrimitiveType { get; }
    public bool IsInitialized { get; private set; }

    public RenderObject(ShaderProgram shaderProgram, Vertex[] vertices, IUniformContext[] uniformContext,
        PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        ShaderProgram = shaderProgram;
        Vertices = vertices;
        UniformContext = uniformContext;
        PrimitiveType = primitiveType;
    }

    public void Initialize<T>(VertexArrayObject vao, VertexBufferObject<T> vbo) where T : unmanaged
    {
        Vao = vao;

        vbo.Bind();
        vbo.BufferData(Vertices);

        vao.Bind();
        
        GL.VertexArrayAttribBinding(vao.Handle, 0, 0);
        GL.EnableVertexArrayAttrib(vao.Handle, 0);
        GL.VertexArrayAttribFormat(vao.Handle, 0, 3, vao.VertexAttributeType, false, 0);

        GL.VertexArrayAttribBinding(vao.Handle, 1, 0);
        GL.EnableVertexArrayAttrib(vao.Handle, 1);
        GL.VertexArrayAttribFormat(vao.Handle, 1, 3, vao.VertexAttributeType, false, 3 * vbo.Sizeof);

        GL.VertexArrayVertexBuffer(vao.Handle, 0, vbo.Handle, IntPtr.Zero, Vertex.Size);

        IsInitialized = true;
    }

    public void UpdateUniform(MainCamera camera)
        => Array.ForEach(UniformContext, item => item.Update(ShaderProgram, camera));
}