namespace cg_2.Source.Wrappers;

public class VertexArrayObject : IDisposable
{
    public VertexAttribType VertexAttributeType { get; }
    public int Handle { get; }

    public VertexArrayObject(VertexAttribType vertexAttribType)
    {
        VertexAttributeType = vertexAttribType;
        Handle = GL.GenVertexArray();
    }

    public void Bind() => GL.BindVertexArray(Handle);

    public void Dispose() => GL.DeleteVertexArray(Handle);
}