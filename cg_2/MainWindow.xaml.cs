namespace cg_2;

public partial class MainWindow
{
    private readonly Camera _mainCamera;
    private readonly VertexBufferArray _vao;
    private readonly VertexBuffer _vbo;
    private readonly IndexBuffer _ibo;
    private readonly ShaderProgram _shaderProgram;
    private mat4 _projectionMatrix;
    private mat4 _viewMatrix;
    private mat4 _modelMatrix;
    private DateTime _lastFrame;
    private float _deltaTime;
    private readonly List<PolygonSection> _sections;
    private readonly List<Transform> _transforms;

    public MainWindow()
    {
        InitializeComponent();
        _mainCamera = new Camera();

        _shaderProgram = new ShaderProgram();
        _vao = new VertexBufferArray();
        _vbo = new VertexBuffer();
        _ibo = new IndexBuffer();

        _projectionMatrix = mat4.identity();
        _viewMatrix = mat4.identity();
        _modelMatrix = mat4.identity();

        _lastFrame = DateTime.Now;
        _deltaTime = 0.0f;

        _sections = new List<PolygonSection> { PolygonSection.ReadJson("Input/Section.json") };
        _transforms = Transform.ReadJson("Input/Transform.json").ToList();
    }

    private void OpenGLControl_OnOpenGLInitialized(object sender, OpenGLRoutedEventArgs args)
    {
        var gl = args.OpenGL;

        gl.Enable(OpenGL.GL_DEPTH_TEST);
        gl.Enable(OpenGL.GL_DOUBLEBUFFER);

        VertexShader vertexShader = new();
        vertexShader.CreateInContext(gl);
        vertexShader.LoadSource("Source/Shaders/shader.vert");

        FragmentShader fragmentShader = new();
        fragmentShader.CreateInContext(gl);
        fragmentShader.LoadSource("Source/Shaders/shader.frag");

        vertexShader.Compile();
        fragmentShader.Compile();

        _shaderProgram.CreateInContext(gl);
        _shaderProgram.AttachShader(vertexShader);
        _shaderProgram.AttachShader(fragmentShader);
        _shaderProgram.Link();

        fragmentShader.DestroyInContext(gl);
        vertexShader.DestroyInContext(gl);

        MakeReplication();

        #region Формируем массив вершин и индексов

        var sectionsCount = _sections.Count;
        var verticesCount = _sections[0].VertexCount;
        var ivertex = (ushort)0;

        var vertices = new float[sectionsCount * verticesCount * 3];
        var indices = new ushort[sectionsCount * verticesCount];
        var facesIndices = new ushort[(sectionsCount - 1) * verticesCount * 4];

        foreach (var vertex in _sections.SelectMany(section => section.Vertices))
        {
            vertices[3 * ivertex] = vertex.x;
            vertices[3 * ivertex + 1] = vertex.y;
            vertices[3 * ivertex + 2] = vertex.z;
            ivertex++;
        }

        for (ushort i = 0; i < sectionsCount * verticesCount; i++) indices[i] = i;

        ivertex = 0;

        for (var i = 0; i < (sectionsCount - 1) * verticesCount; i += verticesCount)
        {
            for (var j = 0; j < verticesCount; j++)
            {
                if (j == verticesCount - 1)
                {
                    facesIndices[ivertex++] = indices[i];
                    facesIndices[ivertex++] = indices[i + verticesCount];
                    facesIndices[ivertex++] = indices[i + j + verticesCount];
                    facesIndices[ivertex++] = indices[i + j];
                }
                else
                {
                    facesIndices[ivertex++] = indices[i + j];
                    facesIndices[ivertex++] = indices[i + j + verticesCount];
                    facesIndices[ivertex++] = indices[i + j + verticesCount + 1];
                    facesIndices[ivertex++] = indices[i + j + 1];
                }
            }
        }

        #endregion

        _vao.Create(gl);
        _vao.Bind(gl);

        _vbo.Create(gl);
        _vbo.Bind(gl);
        gl.BufferData(OpenGL.GL_ARRAY_BUFFER, vertices, OpenGL.GL_STATIC_DRAW);
        gl.VertexAttribPointer(0, 3, OpenGL.GL_FLOAT, false, 3 * sizeof(float), IntPtr.Zero);
        gl.EnableVertexAttribArray(0);
        _vbo.Unbind(gl);

        _ibo.Create(gl);
        _ibo.Bind(gl);
        _ibo.SetData(gl, facesIndices);

        _vao.Unbind(gl);
    }

    private void OpenGLControl_OnOpenGLDraw(object sender, OpenGLRoutedEventArgs args)
    {
        #region Считаем deltaTime

        var currentFrame = DateTime.Now;
        _deltaTime = (currentFrame.Ticks - _lastFrame.Ticks) / 10000000f;
        _lastFrame = currentFrame;

        #endregion

        var gl = args.OpenGL;
        var width = gl.RenderContextProvider.Width;
        var height = gl.RenderContextProvider.Height;

        //gl.PolygonMode(OpenGL.GL_FRONT_AND_BACK, OpenGL.GL_LINE);
        gl.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

        _shaderProgram.Push(gl, null);
        _projectionMatrix = glm.perspective(45.0f, width / (float)height, 0.1f, 100.0f);
        _viewMatrix = glm.lookAt(_mainCamera.Position, _mainCamera.Position + _mainCamera.Front, _mainCamera.Up);

        var modelLoc = _shaderProgram.GetUniformLocation("model");
        var viewLoc = _shaderProgram.GetUniformLocation("view");
        var projectionLoc = _shaderProgram.GetUniformLocation("projection");

        gl.UniformMatrix4(viewLoc, 1, false, _viewMatrix.to_array());
        gl.UniformMatrix4(projectionLoc, 1, false, _projectionMatrix.to_array());

        _vao.Bind(gl);

        gl.UniformMatrix4(modelLoc, 1, false, _modelMatrix.to_array());

        var vertexCount = _sections[0].VertexCount;
        var sectionsCount = _sections.Count;

        for (var i = 0; i < _sections.Count; i++)
        {
            gl.DrawArrays(OpenGL.GL_POLYGON, vertexCount * i, vertexCount);
        }

        _ibo.Bind(gl);
        gl.DrawElements(OpenGL.GL_QUADS, (sectionsCount - 1) * vertexCount * 4, OpenGL.GL_UNSIGNED_SHORT, IntPtr.Zero);

        _shaderProgram.Pop(gl, null);
        _vao.Unbind(gl);
    }

    private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.W:
                _mainCamera.Move(TranslateDirection.Forward, _deltaTime);
                break;
            case Key.S:
                _mainCamera.Move(TranslateDirection.Back, _deltaTime);
                break;
            case Key.A:
                _mainCamera.Move(TranslateDirection.Left, _deltaTime);
                break;
            case Key.D:
                _mainCamera.Move(TranslateDirection.Right, _deltaTime);
                break;
        }
    }

    private void MainWindow_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            var pos = e.GetPosition(this);

            _mainCamera.LookAt((float)pos.X, (float)pos.Y);
        }
        else
        {
            _mainCamera.FirstMouse = true;
        }
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
}