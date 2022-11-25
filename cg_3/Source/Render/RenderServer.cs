﻿using System.Drawing;
using cg_3.Source.Vectors;
using cg_3.Source.Wrappers;
using OpenTK.Graphics.OpenGL4;
using ReactiveUI;

namespace cg_3.Source.Render;

public interface IBaseGraphic
{
    public float DeltaTime { get; }

    public void Render(TimeSpan obj);
    public void DrawPoints(IEnumerable<Vector2D> points);
}

public interface IViewable
{
    public void Draw(IBaseGraphic baseGraphic);
}

public class RenderServer : ReactiveObject, IBaseGraphic
{
    private VertexArrayObject? _vao;
    private ShaderProgram? _shaderProgram;
    public float DeltaTime { get; private set; }

    // public MainCamera Camera { get; }
    public IEnumerable<Vector2D>? Points { get; set; }

    // public RenderServer(MainCamera? camera = null)
    // {
    //     GL.ClearColor(Color.Black);
    //     GL.Enable(EnableCap.DepthTest);
    //     Camera = camera ?? new(CameraMode.Perspective);
    // }

    public RenderServer()
    {
        GL.ClearColor(Color.White);
        GL.Enable(EnableCap.DepthTest);
    }

    public void DrawPoints(IEnumerable<Vector2D> points)
    {
        _vao = new(VertexAttribType.Float);
        var vbo = new VertexBufferObject<float>();
        Points = points;

        vbo.Bind();
        vbo.BufferData(points.ToArray());

        _vao.Bind();

        GL.VertexArrayAttribBinding(_vao.Handle, 0, 0);
        GL.EnableVertexArrayAttrib(_vao.Handle, 0);
        GL.VertexArrayAttribFormat(_vao.Handle, 0, 2, _vao.VertexAttributeType, false, 0);
        GL.VertexArrayVertexBuffer(_vao.Handle, 0, vbo.Handle, IntPtr.Zero, Vector2D.Size * 4);

        _shaderProgram = new();
        _shaderProgram.Initialize("Source/Shaders/shader.vert", "Source/Shaders/shader.frag",
            "Source/Shaders/shader.geom");
    }

    public void Render(TimeSpan deltaTime)
    {
        DeltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (Points is null) return;

        _vao!.Bind();
        _shaderProgram!.Use();
        GL.DrawArrays(PrimitiveType.Points, 0, Points!.Count());
    }
}