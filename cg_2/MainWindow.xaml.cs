using cg_2.Source.Primitives;

namespace cg_2;

public partial class MainWindow
{
    private readonly Camera _camera = new();

    private readonly VertexBufferArray _vao = new(),
        _normalVao = new(),
        _objectVao = new(),
        _lightVao = new();

    private readonly VertexBufferWrapper _vbo = new(new()),
        _normalVbo = new(new()),
        _objectVbo = new(new());

    private readonly ShaderProgramWrapper _shaderProgram = new(new());
    private readonly ShaderProgramWrapper _normalProgram = new(new());
    private readonly ShaderProgramWrapper _textureProgram = new(new());
    private readonly ShaderProgramWrapper _lampProgram = new(new());
    private readonly List<PolygonSection> _sections = new() { PolygonSection.ReadJson("Input/Section.json") };
    private readonly List<Transform> _transforms = Transform.ReadJson("Input/Transform.json").ToList();
    private readonly Texture[] _textures = { new(), new() };
    private readonly vec3 _lightPos = new(1.2f, 3.0f, -5.0f);

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

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.W)) _camera.Move(CameraMovement.Forward, _deltaTime.Result);
        if (e.KeyboardDevice.IsKeyDown(Key.S)) _camera.Move(CameraMovement.Backward, _deltaTime.Result);
        if (e.KeyboardDevice.IsKeyDown(Key.A)) _camera.Move(CameraMovement.Left, _deltaTime.Result);
        if (e.KeyboardDevice.IsKeyDown(Key.D)) _camera.Move(CameraMovement.Right, _deltaTime.Result);
        if (e.KeyboardDevice.IsKeyDown(Key.Space)) _camera.Move(CameraMovement.Up, _deltaTime.Result);
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl)) _camera.Move(CameraMovement.Down, _deltaTime.Result);
    }

    #region OpenGL

    private void OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        var gl = args.OpenGL;
        gl.Enable(OpenGL.GL_DEPTH_TEST);
        gl.Enable(OpenGL.GL_DOUBLEBUFFER);

        #region Загрузка шейдеров

        _shaderProgram.Initialize("Source/Shaders/shader.vert", "Source/Shaders/lighting.frag", gl);
        _normalProgram.Initialize("Source/Shaders/normals.vert", "Source/Shaders/normals.frag", gl);
        _textureProgram.Initialize("Source/Shaders/shader.vert", "Source/Shaders/textures.frag", gl);
        _lampProgram.Initialize("Source/Shaders/object.vert", "Source/Shaders/lamp.frag", gl);

        #endregion

        var cubeVertices = Primitives.Cube;

        MakeReplication();

        #region Формируем массив вершин и нормалей

        var sectionsCount = _sections.Count;
        var verticesCount = _sections[0].VertexCount;
        int k, l, ivertex = 0;

        var listVert = _sections.SelectMany(section => section.Vertices).ToList();
        List<vec3> normals = new();
        List<vec3> normalLines = new();

        // Формируем список координат сечений и боковых граней
        for (int i = 0; i < sectionsCount - 1; i++)
        {
            var vertS1 = _sections[i].Vertices;
            var vertS2 = _sections[i + 1].Vertices;

            for (int j = 0; j < verticesCount; j++)
            {
                if (j == verticesCount - 1)
                {
                    listVert.Add(vertS1[j]);
                    listVert.Add(vertS2[j]);
                    listVert.Add(vertS2[0]);
                    listVert.Add(vertS1[0]);
                }
                else
                {
                    listVert.Add(vertS1[j]);
                    listVert.Add(vertS2[j]);
                    listVert.Add(vertS2[j + 1]);
                    listVert.Add(vertS1[j + 1]);
                }
            }
        }

        // Считаем нормали для каждой грани
        for (int i = 0; i < sectionsCount; i++)
        {
            var sectionVert = _sections[i].Vertices;

            var vector1 = sectionVert[1] - sectionVert[0];
            var vector2 = sectionVert[2] - sectionVert[0];

            var normal = glm.normalize(i == sectionsCount - 1
                ? glm.cross(vector1, vector2)
                : glm.cross(vector2, vector1));

            normals.Add(normal);
        }

        for (int i = 0; i < sectionsCount - 1; i++)
        {
            var vertS1 = _sections[i].Vertices;
            var vertS2 = _sections[i + 1].Vertices;

            for (int j = 0; j < verticesCount; j++)
            {
                vec3 vector1;
                vec3 vector2;

                if (j == verticesCount - 1)
                {
                    vector1 = vertS1[j] - vertS1[0];
                    vector2 = vertS2[0] - vertS1[0];
                }
                else
                {
                    vector1 = vertS1[j] - vertS1[j + 1];
                    vector2 = vertS2[j + 1] - vertS1[j + 1];
                }

                var normal = glm.normalize(glm.cross(vector2, vector1));
                normals.Add(normal);
            }
        }

        // Задаем нормали отрезками
        for (k = 0; k < sectionsCount; k++)
        {
            var sectionVert = _sections[k].Vertices;

            foreach (var t in sectionVert)
            {
                normalLines.Add(t);
                normalLines.Add(t + normals[k]);
            }
        }

        for (int i = 0; i < sectionsCount - 1; i++)
        {
            var vertS1 = _sections[i].Vertices;
            var vertS2 = _sections[i + 1].Vertices;

            for (var j = 0; j < verticesCount; j++, k++)
            {
                if (j == verticesCount - 1)
                {
                    normalLines.Add(vertS1[j]);
                    normalLines.Add(vertS1[j] + normals[k]);
                    normalLines.Add(vertS1[0]);
                    normalLines.Add(vertS1[0] + normals[k]);
                    normalLines.Add(vertS2[j]);
                    normalLines.Add(vertS2[j] + normals[k]);
                    normalLines.Add(vertS2[0]);
                    normalLines.Add(vertS2[0] + normals[k]);
                }
                else
                {
                    normalLines.Add(vertS1[j]);
                    normalLines.Add(vertS1[j] + normals[k]);
                    normalLines.Add(vertS1[j + 1]);
                    normalLines.Add(vertS1[j + 1] + normals[k]);
                    normalLines.Add(vertS2[j]);
                    normalLines.Add(vertS2[j] + normals[k]);
                    normalLines.Add(vertS2[j + 1]);
                    normalLines.Add(vertS2[j + 1] + normals[k]);
                }
            }
        }

        var normalLinesArray = new float[3 * normalLines.Count];
        _normalsCount = normalLinesArray.Length;

        foreach (var (normal, idx) in normalLines.Select((normal, idx) => (normal, idx)))
        {
            normalLinesArray[3 * idx] = normal.x;
            normalLinesArray[3 * idx + 1] = normal.y;
            normalLinesArray[3 * idx + 2] = normal.z;
        }

        // Формируем текстурные координаты
        List<vec2> textureCoords = new();
        l = 0;

        for (int i = 0; i < sectionsCount; i++)
        {
            var sectionVertices = _sections[i].Vertices;
            var xy = MinMaxCoord(sectionVertices);
            var hx = xy[1] - xy[0];
            var hy = xy[3] - xy[2];

            for (k = 0; k < verticesCount; k++, l++)
            {
                var vertex = listVert[l];

                var texX = (vertex.x - xy[0]) / hx;
                var texY = (vertex.y - xy[2]) / hy;

                textureCoords.Add(new vec2(texX, texY));
            }
        }

        for (; l < listVert.Count; l++)
        {
            textureCoords.Add(new vec2(0, 1));
            textureCoords.Add(new vec2(0, 0));
            textureCoords.Add(new vec2(1, 0));
            textureCoords.Add(new vec2(1, 1));
        }

        var vertices = new float[2 * 3 * verticesCount * (5 * sectionsCount - 4) + textureCoords.Count * 2];
        k = 0;
        l = 0;

        for (; k < sectionsCount; k++)
        {
            var normal = normals[k];

            for (int j = 0; j < verticesCount; j++, l++)
            {
                var vertex = listVert[l];
                var texVertex = textureCoords[l];

                vertices[8 * ivertex] = vertex.x;
                vertices[8 * ivertex + 1] = vertex.y;
                vertices[8 * ivertex + 2] = vertex.z;
                vertices[8 * ivertex + 3] = normal.x;
                vertices[8 * ivertex + 4] = normal.y;
                vertices[8 * ivertex + 5] = normal.z;
                vertices[8 * ivertex + 6] = texVertex.x;
                vertices[8 * ivertex + 7] = texVertex.y;
                ivertex++;
            }
        }

        for (; k < normals.Count; k++)
        {
            var normal = normals[k];

            for (int j = 0; j < 4; j++, l++)
            {
                var vertex = listVert[l];
                var texVertex = textureCoords[l];

                vertices[8 * ivertex] = vertex.x;
                vertices[8 * ivertex + 1] = vertex.y;
                vertices[8 * ivertex + 2] = vertex.z;
                vertices[8 * ivertex + 3] = normal.x;
                vertices[8 * ivertex + 4] = normal.y;
                vertices[8 * ivertex + 5] = normal.z;
                vertices[8 * ivertex + 6] = texVertex.x;
                vertices[8 * ivertex + 7] = texVertex.y;
                ivertex++;
            }
        }

        #endregion

        #region Загрузка текстур

        _textures[0].Create(gl, "Resources/Textures/face.png");
        _textures[1].Create(gl, "Resources/Textures/wall.bmp");

        #endregion

        #region Привязка буферов

        _vao.Create(gl);
        _normalVao.Create(gl);
        _objectVao.Create(gl);
        _lightVao.Create(gl);
        _vbo.Create(gl);
        _objectVbo.Create(gl);
        _normalVbo.Create(gl);

        _vao.Bind(gl);
        _vbo.Bind(gl);

        _vbo.SetData(gl, 0, vertices, false, 3, 8 * sizeof(float), 0);
        _vbo.SetData(gl, 1, vertices, false, 3, 8 * sizeof(float), 3 * sizeof(float));
        _vbo.SetData(gl, 2, vertices, false, 2, 8 * sizeof(float), 6 * sizeof(float));
        _vao.Unbind(gl);

        // Для отрисовки нормалей
        _normalVao.Bind(gl);
        _normalVbo.Bind(gl);
        _normalVbo.SetData(gl, 0, normalLinesArray, false, 3, 3 * sizeof(float), 0);
        _normalVao.Unbind(gl);

        _objectVao.Bind(gl);
        _objectVbo.Bind(gl);

        _objectVbo.SetData(gl, 0, cubeVertices, false, 3, 6 * sizeof(float), 0);
        _objectVbo.SetData(gl, 1, cubeVertices, false, 3, 6 * sizeof(float), 3 * sizeof(float));
        _objectVao.Unbind(gl);

        _lightVao.Bind(gl);
        _objectVbo.Bind(gl);

        _objectVbo.SetData(gl, 0, cubeVertices, false, 3, 6 * sizeof(float), 0);
        _lightVao.Unbind(gl);

        #endregion
    }

    private void OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        var gl = args.OpenGL;
        var width = gl.RenderContextProvider.Width;
        var height = gl.RenderContextProvider.Height;

        gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, _isWireframe ? OpenGL.GL_LINE : OpenGL.GL_FILL);
        gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        if (!_isTexturize)
        {
            _shaderProgram.Use();
        }
        else
        {
            _textures[_textureId].Bind(gl);
            _textureProgram.Use();
        }

        var projectionMatrix = _isPerspective
            ? glm.perspective(45.0f, width / (float)height, 0.1f, 100.0f)
            : glm.ortho(-width / 50f, width / 50f, -height / 50f, height / 50f, 0.1f, 100);
        var viewMatrix = glm.lookAt(_camera.Position, _camera.Position + _camera.Front, _camera.Up);
        var modelMatrix = mat4.identity();

        var modelLoc = _shaderProgram.GetUniformLocation("model");
        var viewLoc = _shaderProgram.GetUniformLocation("view");
        var projectionLoc = _shaderProgram.GetUniformLocation("projection");

        var objectColorLoc = _shaderProgram.GetUniformLocation("objectColor");
        var lightColorLoc = _shaderProgram.GetUniformLocation("lightColor");
        var lightPosLoc = _shaderProgram.GetUniformLocation("lightPos");
        var viewPosLoc = _shaderProgram.GetUniformLocation("viewPos");

        gl.Uniform3(objectColorLoc, 1.0f, 0.5f, 0.31f);
        gl.Uniform3(lightColorLoc, 1.0f, 1.0f, 1.0f);
        gl.Uniform3(lightPosLoc, _lightPos.x, _lightPos.y, _lightPos.z);
        gl.Uniform3(viewPosLoc, _camera.Position.x, _camera.Position.y, _camera.Position.z);

        gl.UniformMatrix4(viewLoc, 1, false, viewMatrix.to_array());
        gl.UniformMatrix4(projectionLoc, 1, false, projectionMatrix.to_array());
        gl.UniformMatrix4(modelLoc, 1, false, modelMatrix.to_array());

        var vertexCount = _sections[0].VertexCount;
        var sectionsCount = _sections.Count;

        var i = 0;

        _vao.Bind(gl);

        // Сначала отрисовываем сечения
        for (; i < _sections.Count; i++)
        {
            gl.DrawArrays(OpenGL.GL_POLYGON, vertexCount * i, vertexCount);
        }

        i--;

        // Потом идем до конца массива и рисуем боковые грани
        for (; i < 3 * vertexCount * (5 * sectionsCount - 4); i++)
        {
            gl.DrawArrays(OpenGL.GL_POLYGON, 4 * i, 4);
        }

        // Отрисовка нормалей
        if (_isShowNormals)
        {
            _normalProgram.Use();

            modelLoc = _normalProgram.GetUniformLocation("model");
            viewLoc = _normalProgram.GetUniformLocation("view");
            projectionLoc = _normalProgram.GetUniformLocation("projection");

            gl.UniformMatrix4(viewLoc, 1, false, viewMatrix.to_array());
            gl.UniformMatrix4(projectionLoc, 1, false, projectionMatrix.to_array());
            gl.UniformMatrix4(modelLoc, 1, false, modelMatrix.to_array());

            _normalVao.Bind(gl);
            gl.DrawArrays(OpenGL.GL_LINES, 0, _normalsCount);
        }

        _lampProgram.Use();

        viewLoc = _lampProgram.GetUniformLocation("view");
        gl.UniformMatrix4(viewLoc, 1, false, viewMatrix.to_array());

        projectionLoc = _lampProgram.GetUniformLocation("projection");
        gl.UniformMatrix4(projectionLoc, 1, false, projectionMatrix.to_array());

        var model = mat4.identity();
        model = glm.translate(model, new(_lightPos.x, _lightPos.y, _lightPos.z));
        model = glm.scale(model, new(0.2f));
        modelLoc = _lampProgram.GetUniformLocation("model");
        gl.UniformMatrix4(modelLoc, 1, false, model.to_array());

        _lightVao.Bind(gl);
        gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, 36);

        _deltaTime.Compute();
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

    #endregion

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
            var toCenter = new vec3() - sectionV.MassCenter();
            var translateMatrix = glm.translate(mat4.identity(), toCenter);
            var vertices4 = sectionV.Select(vertex => translateMatrix * new vec4(vertex, 1.0f)).ToList();

            // Выполняем преобразования
            var rotateMatrix = glm.rotate(glm.radians(angle), currTraj);
            vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();

            var dotProduct = glm.dot(currTraj, nextTraj);
            var rotAngle = glm.acos(dotProduct / (currTraj.Norm() * nextTraj.Norm()));
            var axis = glm.cross(nextTraj, currTraj);
            rotateMatrix = axis.Norm() != 0.0f ? glm.rotate(-rotAngle, axis) : mat4.identity();
            var scaleMatrix = glm.scale(mat4.identity(), new vec3(scale));

            vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
            vertices4 = vertices4.Select(vertex => scaleMatrix * vertex).ToList();

            // Возвращаем в исходное положение в мировом пространстве
            translateMatrix = glm.translate(mat4.identity(), sectionV.MassCenter());
            vertices4 = vertices4.Select(vertex => translateMatrix * vertex).ToList();

            translateMatrix = glm.translate(mat4.identity(), currTraj);

            foreach (var vertex in vertices4)
            {
                newSection.Vertices.Add(new vec3(translateMatrix * new vec4(vertex)));
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

                rotateMatrix = glm.rotate(glm.radians(angle), currTraj);
                scaleMatrix = glm.scale(mat4.identity(), new vec3(scale));

                // Переносим сечение в начало координат
                toCenter = new vec3() - sectionV.MassCenter();
                translateMatrix = glm.translate(mat4.identity(), toCenter);
                vertices4 = sectionV.Select(vertex => translateMatrix * new vec4(vertex, 1.0f)).ToList();

                // Выполняем преобразования
                vertices4 = vertices4.Select(vertex => rotateMatrix * vertex).ToList();
                vertices4 = vertices4.Select(vertex => scaleMatrix * vertex).ToList();

                // Возвращаем в исходное положение в мировом пространстве
                translateMatrix = glm.translate(mat4.identity(), sectionV.MassCenter());
                vertices4 = vertices4.Select(vertex => translateMatrix * vertex).ToList();

                translateMatrix = glm.translate(mat4.identity(), currTraj);

                foreach (var vertex in vertices4)
                {
                    newSection.Vertices.Add(new vec3(translateMatrix * new vec4(vertex)));
                }

                _sections.Add(newSection);

                break;
            }
        }
    }

    #endregion

    #region Доп. функции

    private static float[] MinMaxCoord(List<vec3> vertices)
    {
        float minX = 1000, maxX = 0, minY = 1000, maxY = 0;

        foreach (var vertex in vertices)
        {
            if (vertex.x < minX) minX = vertex.x;
            if (vertex.x > maxX) maxX = vertex.x;
            if (vertex.y < minY) minY = vertex.y;
            if (vertex.y > maxY) maxY = vertex.y;
        }

        return new[] { minX, maxX, minY, maxY };
    }

    #endregion
}