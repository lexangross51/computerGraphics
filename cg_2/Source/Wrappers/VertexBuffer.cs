namespace cg_2.Source.Wrappers;

public class VertexBufferObject<T> : IDisposable where T : unmanaged
{
    private Type _type = typeof(T);
    public int Handle { get; }

    public VertexBufferObject() => Handle = GL.GenBuffer();


    public void Dispose() => GL.DeleteBuffer(Handle);
}