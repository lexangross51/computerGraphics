namespace cg_2;

public partial class MainWindow
{
    private readonly MainCamera _camera = new(CameraMode.Perspective);
    private RenderServer _renderServer = default!;
    private IRenderable[] _renderables = default!;
    private ShaderProgram _lightingProgram = default!;
    private ShaderProgram _lampProgram = default!;
    private readonly Vector3 _lightPos = new(0.5f, 0.0f, 0.0f);
    private readonly Vector3 _lightDir = new(0.0f, -1.0f, 0.0f);
    private float _deltaTime;
    private readonly List<PolygonSection> _sections = new() { PolygonSection.ReadJson("Input/Section.json") };

    private readonly List<Transform> _transforms = Transform.ReadJson("Input/Transform.json").ToList();
    // private Vertex[] _sectionsVertices = default!;
    // private Vertex[] _facesVertices = default!;

    public MainWindow() => InitializeComponent();

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.W)) _camera.Move(CameraMovement.Forward, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.S)) _camera.Move(CameraMovement.Backward, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.A)) _camera.Move(CameraMovement.Left, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.D)) _camera.Move(CameraMovement.Right, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.Space)) _camera.Move(CameraMovement.Up, _deltaTime);
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) _camera.Move(CameraMovement.Down, _deltaTime);
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

    private void OnMouseWheel(object sender, MouseWheelEventArgs e) => _camera.Fov -= e.Delta / 100.0f;

    private void OnRender(TimeSpan deltaTime)
    {
        _deltaTime = (float)deltaTime.TotalMilliseconds;
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        _renderServer.Render(_camera);
    }

    private void OnInitialize(object? sender, EventArgs e)
    {
        var mainSettings = new GLWpfControlSettings();
        OpenTkControl.Start(mainSettings);
        OpenTkControl.RenderSize = new(1920, 1080);

        GL.ClearColor(Color.Black);
        GL.Enable(EnableCap.DepthTest);

        var width = (float)OpenTkControl.RenderSize.Width;
        var height = (float)OpenTkControl.RenderSize.Height;
        _camera.AspectRatio = width / height;

        _lightingProgram = new();
        _lightingProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/pointLight.frag");

        _lampProgram = new();
        _lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag");

        var projectionMatrix = _camera.GetProjectionMatrix();
        var viewMatrix = _camera.GetViewMatrix();
        var modelMatrix = Matrix4.CreateTranslation(new(0.5f, 0.0f, -2.0f));
        var modelMatrix1 = Matrix4.CreateScale(0.2f);
        modelMatrix1 *= Matrix4.CreateTranslation(_lightPos);
        var model2 = Matrix4.CreateTranslation(new(0.5f, -2.0f, 0.0f));
        var model3 = Matrix4.CreateTranslation(new(2.0f, 0.0f, 0.0f));

        MakeReplication();

        #region Формирование вершин для сечений и граней

        var sectionsCount = _sections.Count;
        var verticesBySection = _sections[0].VertexCount;
        var facesCount = (sectionsCount - 1) * verticesBySection;

        var sectionsVerticesArray = new Vertex[sectionsCount * verticesBySection];
        var facesVerticesArray = new Vertex[facesCount * 4];
        int idx = 0;

        foreach (var vertex in _sections.SelectMany(section => section.Vertices))
        {
            sectionsVerticesArray[idx++].Position = vertex;
        }

        idx = 0;
        for (int i = 0; i < sectionsCount - 1; i++)
        {
            var vertices1 = _sections[i].Vertices;
            var vertices2 = _sections[i + 1].Vertices;

            for (int j = 0; j < verticesBySection; j++)
            {
                if (j == verticesBySection - 1)
                {
                    facesVerticesArray[idx++].Position = vertices1[j];
                    facesVerticesArray[idx++].Position = vertices2[j];
                    facesVerticesArray[idx++].Position = vertices2[0];
                    facesVerticesArray[idx++].Position = vertices1[0];
                }
                else
                {
                    facesVerticesArray[idx++].Position = vertices1[j];
                    facesVerticesArray[idx++].Position = vertices2[j];
                    facesVerticesArray[idx++].Position = vertices2[j + 1];
                    facesVerticesArray[idx++].Position = vertices1[j + 1];
                }
            }
        }

        // Считаем нормали для каждой грани
        idx = 0;
        for (int i = 0; i < sectionsCount; i++)
        {
            var sectionVertices = _sections[i].Vertices;

            var vector1 = sectionVertices[1] - sectionVertices[0];
            var vector2 = sectionVertices[^1] - sectionVertices[0];

            var normal = Vector3.Normalize(i == sectionsCount - 1
                ? Vector3.Cross(vector1, vector2)
                : Vector3.Cross(vector2, vector1));

            for (int j = 0; j < verticesBySection; j++)
            {
                sectionsVerticesArray[idx++].Normal = normal;
            }
        }

        idx = 0;
        for (int i = 0; i < sectionsCount - 1; i++)
        {
            var vertices1 = _sections[i].Vertices;
            var vertices2 = _sections[i + 1].Vertices;

            for (int j = 0; j < verticesBySection; j++)
            {
                Vector3 vector1;
                Vector3 vector2;

                if (j == verticesBySection - 1)
                {
                    vector1 = vertices1[j] - vertices1[0];
                    vector2 = vertices2[0] - vertices1[0];
                }
                else
                {
                    vector1 = vertices1[j] - vertices1[j + 1];
                    vector2 = vertices2[j + 1] - vertices1[j + 1];
                }

                var normal = Vector3.Normalize(Vector3.Cross(vector2, vector1));
                facesVerticesArray[idx++].Normal = normal;
            }
        }

        #endregion

        _renderables = new IRenderable[]
        {
            new RenderObject(_lightingProgram, Primitives.Cube(1.0f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix, "model")),
                Lighting.BrightLight((_lightPos, "light.position"),
                    (_lightDir, "light.direction"), "viewPos"),
                Material.GoldMaterial
            }, PrimitiveType.Quads),
            new RenderObject(_lampProgram, Primitives.Cube(1.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (modelMatrix1, "model"))
            }),
            new RenderObject(_lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model2, "model")),
                Lighting.BrightLight((_lightPos, "light.position"),
                    (_lightDir, "light.direction"), "viewPos"),
                Material.GoldMaterial
            }),
            new RenderObject(_lightingProgram, Primitives.Cube(0.5f), new IUniformContext[]
            {
                new Transformation((viewMatrix, "view"), (projectionMatrix, "projection"), (model3, "model")),
                Lighting.BrightLight((_lightPos, "light.position"),
                    (_lightDir, "light.direction"), "viewPos"),
                Material.GoldMaterial
            })
        };

        foreach (var renderable in _renderables)
        {
            renderable.Initialize(new VertexArrayObject(VertexAttribType.Float),
                new VertexBufferObject<float>());
        }

        _renderServer = new(_renderables);
    }

    private void MakeReplication()
    {
        for (var i = 0; i < _transforms.Count - 1; i++)
        {
            PolygonSection newSection = new();

            var sectionV = _sections[i].Vertices;
            var scale = _transforms[i].Scale;
            var angle = _transforms[i].Angle;
            var currTraj = _transforms[i].Trajectory;
            var nextTraj = _transforms[i + 1].Trajectory;

            // Перенесем сечение в начало координат
            var toCenter = new Vector3() - sectionV.MassCenter();
            var translateMatrix = Matrix4.CreateTranslation(toCenter);
            translateMatrix.Transpose();
            var vertices4 = sectionV.Select(vertex => translateMatrix * new Vector4(vertex, 1.0f)).ToList();

            // Выполняем преобразования
            var rotateMatrix = Matrix4.CreateFromAxisAngle(currTraj, (float)(angle * Math.PI / 180.0));
            rotateMatrix.Transpose();
            vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();

            var dotProduct = Vector3.Dot(currTraj, nextTraj);
            var rotAngle = (float)Math.Acos(dotProduct / (currTraj.Length * nextTraj.Length));
            var axis = Vector3.Cross(nextTraj, currTraj);
            rotateMatrix = axis.Length != 0.0f ? Matrix4.CreateFromAxisAngle(axis, -rotAngle) : Matrix4.Identity;
            rotateMatrix.Transpose();
            var scaleMatrix = Matrix4.CreateScale(scale);
            scaleMatrix.Transpose();

            vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
            vertices4 = vertices4.Select(vertex => scaleMatrix * vertex).ToList();

            // Возвращаем в исходное положение в мировом пространстве
            translateMatrix = Matrix4.CreateTranslation(sectionV.MassCenter());
            translateMatrix.Transpose();
            vertices4 = vertices4.Select(vertex => translateMatrix * vertex).ToList();

            translateMatrix = Matrix4.CreateTranslation(currTraj);
            translateMatrix.Transpose();

            foreach (var vertex in vertices4)
            {
                newSection.Vertices.Add(new Vector3(translateMatrix * vertex));
            }

            _sections.Add(newSection);

            // Обработка последней трансформации
            if (i == _transforms.Count - 2)
            {
                newSection = new PolygonSection();

                sectionV = _sections[^1].Vertices;
                currTraj = _transforms[^1].Trajectory;
                scale = _transforms[^1].Scale;
                angle = _transforms[^1].Angle;

                rotateMatrix = Matrix4.CreateFromAxisAngle(currTraj, (float)(angle * Math.PI / 180.0));
                rotateMatrix.Transpose();
                scaleMatrix = Matrix4.CreateScale(scale);
                scaleMatrix.Transpose();

                // Переносим сечение в начало координат
                toCenter = new Vector3() - sectionV.MassCenter();
                translateMatrix = Matrix4.CreateTranslation(toCenter);
                translateMatrix.Transpose();
                vertices4 = sectionV.Select(vertex => translateMatrix * new Vector4(vertex, 1.0f)).ToList();

                // Выполняем преобразования
                vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
                vertices4 = vertices4.Select(vertex => scaleMatrix * vertex).ToList();

                // Возвращаем в исходное положение в мировом пространстве
                translateMatrix = Matrix4.CreateTranslation(sectionV.MassCenter());
                translateMatrix.Transpose();
                vertices4 = vertices4.Select(vertex => translateMatrix * vertex).ToList();

                translateMatrix = Matrix4.CreateTranslation(currTraj);
                translateMatrix.Transpose();

                foreach (var vertex in vertices4)
                {
                    newSection.Vertices.Add(new Vector3(translateMatrix * vertex));
                }

                _sections.Add(newSection);

                break;
            }
        }
    }
}