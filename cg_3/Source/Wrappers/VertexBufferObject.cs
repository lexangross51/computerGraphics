namespace cg_3.Source.Wrappers;

public class VertexBufferObject<T> : IDisposable where T : unmanaged
{
    public unsafe int Sizeof => sizeof(T);
    public int Handle { get; }
    public BufferUsageHint TypeDraw { get; init; } = BufferUsageHint.StaticDraw;

    public VertexBufferObject() => Handle = GL.GenBuffer();

    public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);

    public void BufferData(Vector2D[] data)
        => GL.NamedBufferStorage(Handle, Vector2D.Size * data.Length, data, BufferStorageFlags.MapWriteBit);

    public void Dispose() => GL.DeleteBuffer(Handle);
}