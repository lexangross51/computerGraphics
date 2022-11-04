using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_2.ViewModels;

public class DrawingViewModel : ReactiveObject
{
    public IBaseGraphic BaseGraphic { get; }
    public Vector3 LightPosition { get; } // TODO -> reactive attribute dont work, may be need use wrapper
    public Vector3 LightDirection { get; }

    [Reactive] public float X { get; set; } = 1.0f;
    [Reactive] public float Y { get; set; } = 0.0f;
    [Reactive] public float Z { get; set; } = 0.0f;
    public ReactiveCommand<Unit, Unit> InitializeContextRenderCommand { get; }

    public DrawingViewModel(IBaseGraphic baseGraphic)
    {
        BaseGraphic = baseGraphic;
        LightDirection = new(1.0f, 0.0f, 0.0f);
        LightPosition = new(0.0f);
        InitializeContextRenderCommand = ReactiveCommand.Create(() =>
        {
            BaseGraphic.RenderObjects ??= CreateRenderObjects();
        });
        this.WhenAnyValue(x => x.X, x => x.Y, x => x.Z)
            .Subscribe(_ => UpdateUniforms());
    }

    private void UpdateUniforms()
    {
        if (BaseGraphic.RenderObjects is null) return;
        foreach (var @object in BaseGraphic.RenderObjects)
        {
            var uniform = @object.UniformContext.OfType<Lighting>().FirstOrDefault();

            if (uniform is null) return;

            uniform.LightDirContext = uniform.LightDirContext with { Value = new(X, Y, Z) };
        }
    }

    public void OnRender(TimeSpan deltaTime)
    {
        BaseGraphic.DeltaTime = (float)deltaTime.TotalMilliseconds;
        BaseGraphic.RenderObjects ??= CreateRenderObjects();
        BaseGraphic.Render();
    }

    private IRenderable[] CreateRenderObjects()
    {
        ShaderProgram lightingProgram = new();
        lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/pointLight.frag");

        ShaderProgram lampProgram = new();
        lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag");

        var projectionMatrix = BaseGraphic.Camera.GetProjectionMatrix();
        var viewMatrix = BaseGraphic.Camera.GetViewMatrix();
        var modelMatrix = Matrix4.CreateTranslation(new(0.5f, 0.0f, -2.0f));
        var modelMatrix1 = Matrix4.CreateScale(0.2f);
        modelMatrix1 *= Matrix4.CreateTranslation(LightPosition);
        var model2 = Matrix4.CreateTranslation(new(0.5f, -2.0f, 0.0f));
        var model3 = Matrix4.CreateTranslation(new(2.0f, 0.0f, 0.0f));

        var renderObjects = new IRenderable[]
        {
            new RenderObject(lightingProgram, Primitives.Cube(1.0f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                Lighting.BrightLight((LightPosition, "light.position"),
                    (LightDirection, "light.direction"), "viewPos"),
                Material.GoldMaterial
            }),
            new RenderObject(lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model2, "model")),
                Lighting.BrightLight((LightPosition, "light.position"),
                    (LightDirection, "light.direction"), "viewPos"),
                Material.GoldMaterial
            }),
            new RenderObject(lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model3, "model")),
                Lighting.BrightLight((LightPosition, "light.position"),
                    (LightDirection, "light.direction"), "viewPos"),
                Material.GoldMaterial
            }),
            new RenderObject(lampProgram, Primitives.Cube(1.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix1, "model"))
            })
        };

        foreach (var @object in renderObjects)
        {
            @object.Initialize(new VertexArrayObject(VertexAttribType.Float),
                new VertexBufferObject<float>());
        }

        return renderObjects;
    }
}