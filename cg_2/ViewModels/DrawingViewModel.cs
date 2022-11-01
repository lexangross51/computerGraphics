using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace cg_2.ViewModels;

public class DrawingViewModel : ReactiveObject
{
    public IBaseGraphic BaseGraphic { get; }
    [Reactive] public Vector3 LightDirection { get; set; } = new(1.0f, 0.0f, 0.0f);
    [Reactive] public Vector3 LightPosition { get; set; } = new(0.0f, 0.0f, 0.0f);

    public DrawingViewModel(IBaseGraphic baseGraphic)
    {
        BaseGraphic = baseGraphic;
        this.WhenAnyValue(x => x.LightDirection, x => x.LightDirection).Subscribe(_ => UpdateUniforms());
    }

    private void UpdateUniforms()
    {
        throw new NotImplementedException();
    }

    public void OnRender(TimeSpan deltaTime)
    {
        BaseGraphic.DeltaTime = (float)deltaTime.TotalMilliseconds;
        CreateRenderObjects();
        BaseGraphic.Render();
    }

    private void CreateRenderObjects()
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

        BaseGraphic.RenderObjects = renderObjects;
    }
}