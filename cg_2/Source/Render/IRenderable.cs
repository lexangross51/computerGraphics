using cg_2.Source.Descriptors;

namespace cg_2.Source.Render;

public interface IRenderable
{
    public ShaderProgram ShaderProgram { get; }
    public VertexArrayObject Vao { get; }
    public float[] Vertices { get; }
    public IUniformContext[] UniformContext { get; }
    public InstanceDescriptor Descriptor { get; }
    public PrimitiveType PrimitiveType { get; }
    public Color4 Color { get; }

    public void Initialize<T>(VertexArrayObject vao, VertexBufferObject<T> vbo) where T : unmanaged;

    public void UpdateUniform(MainCamera camera);
}