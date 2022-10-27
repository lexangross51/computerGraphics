namespace cg_2.Source.Render;

public class Instance : IRenderable
{
    public ShaderProgram ShaderProgram { get; }
    public VertexArrayObject? Vao { get; private set; }
    public float[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public InstanceDescriptor Descriptor { get; }
    public PrimitiveType PrimitiveType { get; }
    public bool IsInitialized { get; private set; }

    public Instance(ShaderProgram shaderProgram, float[] vertices, IUniformContext[] uniformContext,
        InstanceDescriptor descriptor,
        PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        ShaderProgram = shaderProgram;
        Vertices = vertices;
        UniformContext = uniformContext;
        Descriptor = descriptor;
        PrimitiveType = primitiveType;
    }

    public void Initialize<T>(VertexArrayObject vao, VertexBufferObject<T> vbo) where T : unmanaged
    {
        Vao = vao;

        vao.Bind();
        vbo.Bind();
        vbo.BufferData(Vertices);

        // for (int i = 0; i < Descriptor.AttributeCount; i++)
        // {
        //     GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        //     GL.EnableVertexAttribArray(i);
        // } TODO -> можно использовать GL.EnableVertexArrayAttrib
        // GL.VertexArrayAttribFormat, но придется хранить обертку над массивом Vertices, так что это вариант


        // GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * vbo.Sizeof, 0);
        // GL.EnableVertexAttribArray(0);
        //
        // GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * vbo.Sizeof, 3);
        // GL.EnableVertexAttribArray(1);

        GL.VertexArrayAttribBinding(vao.Handle, 0, 0);
        GL.EnableVertexArrayAttrib(vao.Handle, 0);
        GL.VertexArrayAttribFormat(vao.Handle, 0, 3, VertexAttribType.Float, false, 0);

        GL.VertexArrayAttribBinding(vao.Handle, 1, 0);
        GL.EnableVertexArrayAttrib(vao.Handle, 1);
        GL.VertexArrayAttribFormat(vao.Handle, 1, 3, VertexAttribType.Float, false, 3);

        GL.VertexArrayVertexBuffer(vao.Handle, 0, vbo.Handle, IntPtr.Zero, 24);

        IsInitialized = true;
    }

    public void UpdateUniform(MainCamera camera)
        => Array.ForEach(UniformContext, item => item.Update(ShaderProgram, camera));
}