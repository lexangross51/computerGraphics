using cg_2.Source.Render;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace cg_2;

public partial class MainWindow
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private ShaderProgram _lightingProgram;
    private ShaderProgram _lampProgram;
    private IRenderable[] _renderables;
    private RenderServer _renderServer;
    private readonly List<PolygonSection> _sections = new() { PolygonSection.ReadJson("Input/Section.json") };
    private readonly List<Transform> _transforms = Transform.ReadJson("Input/Transform.json").ToList();
    private readonly Vector3d _lightPos = new(1.2f, 1.0f, 2.0f);
    private float _parameter;

    private readonly IEnumerable<string> _collectionTextures = new List<string>
        { "Нет текстуры", "Текстура_1", "Текстура_2" };

    private readonly DeltaTime _deltaTime = new();
    private bool _isWireframe;
    private bool _isPerspective;
    private bool _isTexturize;
    private int _textureId;
    private bool _isShowNormals;
    private bool _isSmoothedNormals;
    private int _normalsCount;

    public MainWindow()
    {
        InitializeComponent();
        TextureName.ItemsSource = _collectionTextures;
        TextureName.SelectedItem = "Нет текстуры";
    }

    private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _camera.Move(CameraMovement.Forward, _deltaTime.Result);
                break;
            case Key.S:
                _camera.Move(CameraMovement.Backward, _deltaTime.Result);
                break;
            case Key.A:
                _camera.Move(CameraMovement.Left, _deltaTime.Result);
                break;
            case Key.D:
                _camera.Move(CameraMovement.Right, _deltaTime.Result);
                break;
            case Key.Space:
                _camera.Move(CameraMovement.Up, _deltaTime.Result);
                break;
            case Key.LeftCtrl:
                _camera.Move(CameraMovement.Down, _deltaTime.Result);
                break;
        }
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            _camera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            _camera.FirstMouse = true;
        }
    }

    #region Взаимодействие с интерфейсом

    private void RadioButtonChecked(object sender, RoutedEventArgs e)
    {
        var radioButton = sender as RadioButton;

        _isWireframe = radioButton!.Name switch
        {
            "WireframeMode" => true,
            "LayoutMode" => false,
            _ => _isWireframe
        };
        _isPerspective = radioButton.Name switch
        {
            "PerspectiveMode" => true,
            "OrthographicMode" => false,
            _ => _isPerspective
        };

        _isSmoothedNormals = radioButton.Name switch
        {
            "NormalsButton" => false,
            "SmoothedNormalsButton" => true,
            _ => _isSmoothedNormals
        };
    }

    private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        var slider = sender as Slider;

        if (slider!.Name == "CameraSpeed")
        {
            _camera.Speed = (float)e.NewValue;
        }
        else
        {
            _camera.Sensitivity = (float)e.NewValue;
        }
    }

    private void CheckBoxChecked(object sender, RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;

        _isShowNormals = checkBox!.IsChecked ?? false;
    }

    private void ComboBoxSelected(object sender, RoutedEventArgs e)
    {
        var comboBox = sender as ComboBox;
        var value = comboBox!.SelectedItem.ToString();

        if (value == "Нет текстуры")
        {
            _isTexturize = false;
        }
        else
        {
            _isTexturize = true;
            _textureId = value == "Текстура_1" ? 0 : 1;
        }
    }

    #endregion

    #region Тиражирование

    // private void MakeReplication()
    // {
    //     for (var i = 0; i < _transforms.Count - 1; i++)
    //     {
    //         PolygonSection newSection = new();
    //
    //         var sectionV = _sections[i].Vertices;
    //         var scale = _transforms[i].Scale;
    //         var angle = _transforms[i].Angle;
    //         var currTraj = _transforms[i].Trajectory;
    //         var nextTraj = _transforms[i + 1].Trajectory;
    //
    //         // Перенесем сечение в начало координат
    //         var toCenter = new vec3() - sectionV.MassCenter();
    //         var translateMatrix = glm.translate(mat4.identity(), toCenter);
    //         var vertices4 = sectionV.Select(vertex => translateMatrix * new vec4(vertex, 1.0f)).ToList();
    //
    //         // Выполняем преобразования
    //         var rotateMatrix = glm.rotate(glm.radians(angle), currTraj);
    //         vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
    //
    //         var dotProduct = glm.dot(currTraj, nextTraj);
    //         var rotAngle = glm.acos(dotProduct / (currTraj.Norm() * nextTraj.Norm()));
    //         var axis = glm.cross(nextTraj, currTraj);
    //         rotateMatrix = axis.Norm() != 0.0f ? glm.rotate(-rotAngle, axis) : mat4.identity();
    //         var scaleMatrix = glm.scale(mat4.identity(), new vec3(scale));
    //
    //         vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
    //         vertices4 = vertices4.Select(vertex => scaleMatrix * vertex).ToList();
    //
    //         // Возвращаем в исходное положение в мировом пространстве
    //         translateMatrix = glm.translate(mat4.identity(), sectionV.MassCenter());
    //         vertices4 = vertices4.Select(vertex => translateMatrix * vertex).ToList();
    //
    //         translateMatrix = glm.translate(mat4.identity(), currTraj);
    //
    //         foreach (var vertex in vertices4)
    //         {
    //             newSection.Vertices.Add(new vec3(translateMatrix * new vec4(vertex)));
    //         }
    //
    //         _sections.Add(newSection);
    //
    //         // Обработка последней трансформации
    //         if (i == _transforms.Count - 2)
    //         {
    //             newSection = new PolygonSection();
    //
    //             sectionV = _sections[^1].Vertices;
    //             currTraj = _transforms[^1].Trajectory;
    //             scale = _transforms[^1].Scale;
    //             angle = _transforms[^1].Angle;
    //
    //             rotateMatrix = glm.rotate(glm.radians(angle), currTraj);
    //             scaleMatrix = glm.scale(mat4.identity(), new vec3(scale));
    //
    //             // Переносим сечение в начало координат
    //             toCenter = new vec3() - sectionV.MassCenter();
    //             translateMatrix = glm.translate(mat4.identity(), toCenter);
    //             vertices4 = sectionV.Select(vertex => translateMatrix * new vec4(vertex, 1.0f)).ToList();
    //
    //             // Выполняем преобразования
    //             vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
    //             vertices4 = vertices4.Select(vertex => scaleMatrix * vertex).ToList();
    //
    //             // Возвращаем в исходное положение в мировом пространстве
    //             translateMatrix = glm.translate(mat4.identity(), sectionV.MassCenter());
    //             vertices4 = vertices4.Select(vertex => translateMatrix * vertex).ToList();
    //
    //             translateMatrix = glm.translate(mat4.identity(), currTraj);
    //
    //             foreach (var vertex in vertices4)
    //             {
    //                 newSection.Vertices.Add(new vec3(translateMatrix * new vec4(vertex)));
    //             }
    //
    //             _sections.Add(newSection);
    //
    //             break;
    //         }
    //     }
    // }

    #endregion

    #region Доп. функции

    // private static float[] MinMaxCoord(List<vec3> vertices)
    // {
    //     float minX = 1000, maxX = 0, minY = 1000, maxY = 0;
    //
    //     foreach (var vertex in vertices)
    //     {
    //         if (vertex.x < minX) minX = vertex.x;
    //         if (vertex.x > maxX) maxX = vertex.x;
    //         if (vertex.y < minY) minY = vertex.y;
    //         if (vertex.y > maxY) maxY = vertex.y;
    //     }
    //
    //     return new[] { minX, maxX, minY, maxY };
    // }

    #endregion

    private void Render(TimeSpan deltaTime)
    {
        GL.ClearColor(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
    }

    private void Initialize(object? sender, EventArgs e)
    {
        var width = (float)Control.Width;
        var height = (float)Control.Height;

        var projectionMatrix = _isPerspective
            ? Matrix4.CreatePerspectiveFieldOfView(45.0f, width / (float)height, 0.1f, 100.0f)
            : glm.ortho(-width / 50f, width / 50f, -height / 50f, height / 50f, 0.1f, 100);
        var viewMatrix = glm.lookAt(_camera.Position, _camera.Position + _camera.Front, _camera.Up);
        var modelMatrix = mat4.identity();

        _renderables = new IRenderable[]
        {
            new SolidInstance(_lightingProgram, Primitives.Cube, new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                new Lighting(new(new(Color.Coral), "objectColor"), (new(1.0f, 1.0f, 1.0f), "lightColor"),
                    (_lightPos, "lightPos"))
            })
        };

        _renderServer = new(_renderables);
        _renderServer.Load();
    }
}