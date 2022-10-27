namespace cg_2.Source.Render;

public class RenderObject : IRenderable
{
    public ShaderProgram ShaderProgram { get; }
    public VertexArrayObject? Vao { get; private set; }
    public float[] Vertices { get; }
    public IUniformContext[] UniformContext { get; } 
    public PrimitiveType PrimitiveType { get; }
    public bool IsInitialized { get; private set; }

    public RenderObject(ShaderProgram shaderProgram, float[] vertices, IUniformContext[] uniformContext,
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
        GL.VertexAttribPointer(0, 3, vao.VertexAttributePointerType, false, 6 * vbo.Sizeof, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, vao.VertexAttributePointerType, false, 6 * vbo.Sizeof, 3 * vbo.Sizeof);
        GL.EnableVertexAttribArray(1);

        // its work too
        // GL.VertexArrayAttribBinding(vao.Handle, 0, 0);
        // GL.EnableVertexArrayAttrib(vao.Handle, 0);
        // GL.VertexArrayAttribFormat(vao.Handle, 0, 3, VertexAttribType.Float, false, 0);
        //
        // GL.VertexArrayAttribBinding(vao.Handle, 1, 0);
        // GL.EnableVertexArrayAttrib(vao.Handle, 1);
        // GL.VertexArrayAttribFormat(vao.Handle, 1, 3, VertexAttribType.Float, false, 3 * sizeof(float));
        //
        // GL.VertexArrayVertexBuffer(vao.Handle, 0, vbo.Handle, IntPtr.Zero, 24);

        IsInitialized = true;
    }

    public void UpdateUniform(MainCamera camera)
        => Array.ForEach(UniformContext, item => item.Update(ShaderProgram, camera));
}