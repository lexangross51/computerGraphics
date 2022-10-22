namespace cg_2.Source.Wrappers;

public class ShaderProgramWrapper
{
    private readonly ShaderProgram _shaderProgram;

    public bool? LinkStatus => _shaderProgram.LinkStatus;
    public uint ProgramObject => _shaderProgram.ProgramObject;
    public OpenGL CurrentOpenGLContext { get; }

    public ShaderProgramWrapper(ShaderProgram shaderProgram, OpenGL glContext)
    {
        _shaderProgram = shaderProgram;
        CurrentOpenGLContext = glContext;

        _shaderProgram.CreateInContext(CurrentOpenGLContext);
    }

    public void AttachShaders(string vertexShaderPath, string fragmentShaderPath)
    {
        var vertexShader = new VertexShader();
        vertexShader.CreateInContext(CurrentOpenGLContext);
        vertexShader.LoadSource(vertexShaderPath);

        var fragmentShader = new FragmentShader();
        fragmentShader.CreateInContext(CurrentOpenGLContext);
        fragmentShader.LoadSource(fragmentShaderPath);

        _shaderProgram.AttachShader(vertexShader);
        _shaderProgram.AttachShader(fragmentShader);

        vertexShader.Compile();
        fragmentShader.Compile();

        _shaderProgram.Link();

        vertexShader.DestroyInContext(CurrentOpenGLContext);
        fragmentShader.DestroyInContext(CurrentOpenGLContext);
    }

    public void SetUniform(string name, int value)
    {
        int location = CurrentOpenGLContext.GetUniformLocation(_shaderProgram.ProgramObject, name);

        if (location == -1) throw new Exception($"{name} uniform not found on shader");

        CurrentOpenGLContext.Uniform1(location, value);
    }

    public void SetUniform(string name, float value)
    {
        int location = CurrentOpenGLContext.GetUniformLocation(_shaderProgram.ProgramObject, name);

        if (location == -1) throw new Exception($"{name} uniform not found on shader");

        CurrentOpenGLContext.Uniform1(location, value);
    }

    public void SetUniform(string name, float x, float y, float z)
    {
        int location = CurrentOpenGLContext.GetUniformLocation(_shaderProgram.ProgramObject, name);

        if (location == -1) throw new Exception($"{name} uniform not found on shader");

        CurrentOpenGLContext.Uniform3(location, x, y, z);
    }

    public void SetUniform(string name, float x, float y, float z, float w)
    {
        int location = CurrentOpenGLContext.GetUniformLocation(_shaderProgram.ProgramObject, name);

        if (location == -1) throw new Exception($"{name} uniform not found on shader");

        CurrentOpenGLContext.Uniform4(location, x, y, z, w);
    }

    public void SetUniform(string name, mat4 value)
    {
        int location = CurrentOpenGLContext.GetUniformLocation(_shaderProgram.ProgramObject, name);

        if (location == -1) throw new Exception($"{name} uniform not found on shader");

        CurrentOpenGLContext.UniformMatrix4(location, 1, false, value.to_array());
    }

    public void Push() => CurrentOpenGLContext.UseProgram(_shaderProgram.ProgramObject);

    public void Pop() => CurrentOpenGLContext.UseProgram(0U);
}