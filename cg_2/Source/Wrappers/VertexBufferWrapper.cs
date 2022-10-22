namespace cg_2.Source.Wrappers;

public class VertexBufferWrapper
{
    private readonly VertexBuffer _vertexBuffer;

    public VertexBufferWrapper(VertexBuffer vertexBuffer) => _vertexBuffer = vertexBuffer;

    public void Create(OpenGL gl) => _vertexBuffer.Create(gl);

    public void SetData(
        OpenGL gl,
        uint attributeIndex,
        float[] rawData,
        bool isNormalised,
        int size,
        int stride,
        int ptr)
    {
        gl.BufferData(34962U, rawData, 35044U);
        gl.VertexAttribPointer(attributeIndex, size, 5126U, isNormalised, stride * sizeof(float),
            ptr == 0 ? IntPtr.Zero : new IntPtr(ptr * sizeof(float)));
        gl.EnableVertexAttribArray(attributeIndex);
    }

    public void Bind(OpenGL gl) => _vertexBuffer.Bind(gl);

    public void Unbind(OpenGL gl) => _vertexBuffer.Unbind(gl);

    public bool IsCreated() => _vertexBuffer.IsCreated();

    public uint VertexBufferObject => _vertexBuffer.VertexBufferObject;
}