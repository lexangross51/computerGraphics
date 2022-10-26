namespace cg_2.Source.Render;

public class Instance : IRenderable
{
    public ShaderProgram ShaderProgram { get; }
    public VertexArrayObject? Vao { get; private set; }
    public float[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public InstanceDescriptor Descriptor { get; }
    public Color4 Color { get; }
    public PrimitiveType PrimitiveType { get; }

    public Instance(ShaderProgram shaderProgram, float[] vertices, IUniformContext[] uniformContext,
        InstanceDescriptor descriptor,
        Color4 color, PrimitiveType primitiveType = PrimitiveType.Triangles)
    {
        ShaderProgram = shaderProgram;
        Vertices = vertices;
        UniformContext = uniformContext;
        Descriptor = descriptor;
        PrimitiveType = primitiveType;
        Color = color;
    }

    public void Initialize<T>(VertexArrayObject vao, VertexBufferObject<T> vbo) where T : unmanaged
    {
        Vao = vao;

        vao.Bind();
        vbo.Bind();
        vbo.BufferData(Vertices.Length, Vertices);

        // for (int i = 0; i < Descriptor.AttributeCount; i++)
        // {
        //     GL.VertexAttribPointer(i, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        //     GL.EnableVertexAttribArray(i);
        // } TODO -> можно использовать GL.EnableVertexArrayAttrib
        // GL.VertexArrayAttribFormat, но придется хранить обертку над массивом Vertices, так что это вариант

        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 6 * vbo.Sizeof, 0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 6 * vbo.Sizeof, 3);
        GL.EnableVertexAttribArray(1);
    }

    public void UpdateUniform(MainCamera camera)
        => Array.ForEach(UniformContext, item => item.Update(ShaderProgram, camera));
}