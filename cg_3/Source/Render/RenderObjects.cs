using cg_3.Source.Vectors;
using cg_3.Source.Wrappers;
using OpenTK.Graphics.OpenGL4;

namespace cg_3.Source.Render;

public interface IRenderable
{
    VertexArrayObject Vao { get; }
    ShaderProgram? ShaderProgram { get; }
    IUniformContext UniformContext { get; }
    PrimitiveType PrimitiveType { get; }
    public int VerticesSize { get; }
}

public abstract class RenderUnit : IRenderable
{
    public VertexArrayObject Vao { get; }
    public ShaderProgram? ShaderProgram { get; protected set; }
    public IUniformContext UniformContext { get; }
    public PrimitiveType PrimitiveType { get; }
    public int VerticesSize { get; protected set; }

    protected RenderUnit(VertexArrayObject? vao = null, ShaderProgram? shaderProgram = null,
        IUniformContext? uniformContext = null, PrimitiveType primitiveType = PrimitiveType.Points)
    {
        Vao = vao ?? new VertexArrayObject(VertexAttribType.Float);
        ShaderProgram = shaderProgram ?? ShaderProgram.StandartProgram();
        UniformContext = uniformContext ?? Transformation.DefaultUniformTransformationContext;
        PrimitiveType = primitiveType;
    }
}

public class RenderObject : RenderUnit
{
    public RenderObject(VertexArrayObject? vao = null, ShaderProgram? shaderProgram = null,
        IUniformContext? uniformContext = null, PrimitiveType primitiveType = PrimitiveType.Points) : base(vao,
        shaderProgram, uniformContext,
        primitiveType)
    {
    }

    public IRenderable Initialize(IEnumerable<Vector2D> points)
    {
        var vbo = new VertexBufferObject<float>();
        VerticesSize = points.Count();
        vbo.Bind();
        vbo.BufferData(points.ToArray());
        Vao.Bind();

        ShaderProgram = new(); // TODO
        ShaderProgram.Initialize("Source/Shaders/shader.vert", "Source/Shaders/shader.frag");

        GL.VertexArrayAttribBinding(Vao.Handle, 0, 0);
        GL.EnableVertexArrayAttrib(Vao.Handle, 0);
        GL.VertexArrayAttribFormat(Vao.Handle, 0, 2, Vao.VertexAttributeType, false, 0);
        GL.VertexArrayVertexBuffer(Vao.Handle, 0, vbo.Handle, IntPtr.Zero, Vector2D.Size);
        return this;
    }

    public class RenderObject1 : RenderUnit
    {
        public RenderObject1(VertexArrayObject? vao = null, ShaderProgram? shaderProgram = null,
            IUniformContext? uniformContext = null, PrimitiveType primitiveType = PrimitiveType.Points) : base(vao,
            shaderProgram, uniformContext,
            primitiveType)
        {
        }

        public IRenderable Initialize(IEnumerable<Vector2D> points)
        {
            var vbo = new VertexBufferObject<float>();
            VerticesSize = points.Count();
            vbo.Bind();
            vbo.BufferData(points.ToArray());
            Vao.Bind();
            
            ShaderProgram = new(); // TODO
            ShaderProgram.Initialize("Source/Shaders/shader1.vert", "Source/Shaders/shader.frag");

                GL.VertexArrayAttribBinding(Vao.Handle, 0, 0);
            GL.EnableVertexArrayAttrib(Vao.Handle, 0);
            GL.VertexArrayAttribFormat(Vao.Handle, 0, 2, Vao.VertexAttributeType, false, 0);
            GL.VertexArrayVertexBuffer(Vao.Handle, 0, vbo.Handle, IntPtr.Zero, Vector2D.Size);
            return this;
        }
    }
}