using cg_2.Views.Windows;

namespace cg_2.ViewModels;

public class MainViewModel : ReactiveObject, IViewable
{
    public ReactiveCommand<Unit, Unit> OpenWindow { get; }

    public MainViewModel()
    {
        OpenWindow = ReactiveCommand.Create(() =>
        {
            var dlg = new Window1();
            dlg.Show();
        });
    }

    public void Draw(IBaseGraphic baseGraphic)
        {
            ShaderProgram lightingProgram = new();
            lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/pointLight.frag");

            ShaderProgram lampProgram = new();
            lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag");

            var projectionMatrix = baseGraphic.Camera.GetProjectionMatrix();
            var viewMatrix = baseGraphic.Camera.GetViewMatrix();
            var modelMatrix = Matrix4.CreateTranslation(new(0.5f, 0.0f, -2.0f));
            var modelMatrix1 = Matrix4.CreateScale(0.2f);
            modelMatrix1 *= Matrix4.CreateTranslation(Vector3.UnitX);
            var model2 = Matrix4.CreateTranslation(new(0.5f, -2.0f, 0.0f));
            var model3 = Matrix4.CreateTranslation(new(2.0f, 0.0f, 0.0f));

            var renderObjects = new IRenderable[]
            {
                new RenderObject(lightingProgram, Primitives.Cube(1.0f), new IUniformContext[]
                {
                    new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                    Lighting.BrightLight((Vector3.UnitX, "light.position"),
                        (new(1.0f, 0.0f, 0.0f), "light.direction"), "viewPos"),
                    Material.GoldMaterial
                }),
                new RenderObject(lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
                {
                    new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model2, "model")),
                    Lighting.BrightLight((Vector3.UnitX, "light.position"),
                        (new(1.0f, 0.0f, 0.0f), "light.direction"), "viewPos"),
                    Material.GoldMaterial
                }),
                new RenderObject(lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
                {
                    new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model3, "model")),
                    Lighting.BrightLight((Vector3.UnitX, "light.position"),
                        (new(1.0f, 0.0f, 0.0f), "light.direction"), "viewPos"),
                    Material.GoldMaterial
                }),
                new RenderObject(lampProgram, Primitives.Cube(1.5f), new IUniformContext[]
                {
                    new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix1, "model"))
                })
            };

            baseGraphic.Draw(renderObjects);
        }
    }