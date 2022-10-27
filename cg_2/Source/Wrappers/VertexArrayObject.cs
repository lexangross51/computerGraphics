namespace cg_2.Source.Wrappers;

public class VertexArrayObject : IDisposable
{
    public VertexAttribPointerType VertexAttributePointerType { get; }
    public int Handle { get; }

    public VertexArrayObject(VertexAttribPointerType vertexAttribPointerType)
    {
        VertexAttributePointerType = vertexAttribPointerType;
        Handle = GL.GenVertexArray();
    }

    public void Bind() => GL.BindVertexArray(Handle);

    public void Dispose() => GL.DeleteVertexArray(Handle);
}