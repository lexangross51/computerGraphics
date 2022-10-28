namespace cg_2.Source.Wrappers;

public class VertexBufferObject<T> : IDisposable where T : unmanaged
{
    public unsafe int Sizeof => sizeof(T);
    public int Handle { get; }
    public BufferUsageHint TypeDraw { get; init; } = BufferUsageHint.StaticDraw;

    public VertexBufferObject() => Handle = GL.GenBuffer();

    public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);

    public void BufferData(Vertex[] data)
        => GL.NamedBufferStorage(Handle, Vertex.Size * data.Length, data, BufferStorageFlags.MapWriteBit);

    public void Dispose() => GL.DeleteBuffer(Handle);
}