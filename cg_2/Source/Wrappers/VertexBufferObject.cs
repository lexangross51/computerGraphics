namespace cg_2.Source.Wrappers;

public class VertexBufferObject<T> : IDisposable where T : unmanaged
{
    public unsafe int Stride => sizeof(T);
    public int Handle { get; }
    public BufferUsageHint TypeDraw { get; init; } = BufferUsageHint.StaticDraw;

    public VertexBufferObject() => Handle = GL.GenBuffer();

    public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);

    public unsafe void BufferData(int size, float[] data)
        => GL.BufferData(BufferTarget.ArrayBuffer, size * sizeof(T), data, TypeDraw);

    public void Dispose() => GL.DeleteBuffer(Handle);
}