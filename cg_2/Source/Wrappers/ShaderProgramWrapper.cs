namespace cg_2.Source.Wrappers;

public class ShaderProgramWrapper
{
    private readonly ShaderProgram _shaderProgram;

    public OpenGL? GlContext { get; private set; }

    public ShaderProgramWrapper(ShaderProgram shaderProgram) => _shaderProgram = shaderProgram;

    public void Initialize(string vertexShaderPath, string fragmentShaderPath, OpenGL glContext)
    {
        GlContext = glContext;
        _shaderProgram.CreateInContext(glContext);

        VertexShader vertexShader = new();
        vertexShader.CreateInContext(glContext);
        vertexShader.LoadSource(vertexShaderPath);

        FragmentShader fragmentShader = new();
        fragmentShader.CreateInContext(glContext);
        fragmentShader.LoadSource(fragmentShaderPath);

        vertexShader.Compile();
        fragmentShader.Compile();

        _shaderProgram.AttachShader(vertexShader);
        _shaderProgram.AttachShader(fragmentShader);
        _shaderProgram.Link();

        vertexShader.DestroyInContext(glContext);
        fragmentShader.DestroyInContext(glContext);
    }

    public void Use() => _shaderProgram.Push(_shaderProgram.CurrentOpenGLContext, null);

    public int GetUniformLocation(string name)
        => _shaderProgram.CurrentOpenGLContext.GetUniformLocation(_shaderProgram.ProgramObject, name);
}