namespace cg_2.Source.Wrappers;

public class ShaderProgramWrapper
{
    private readonly ShaderProgram _shaderProgram;
    private readonly OpenGL _glContext;
    public bool? LinkStatus => _shaderProgram.LinkStatus;

    public ShaderProgramWrapper(ShaderProgram shaderProgram, OpenGL glContext)
    {
        _shaderProgram = shaderProgram;
        _glContext = glContext;
        _shaderProgram.CreateInContext(_glContext);
    }

    public void AttachShaders(VertexShader vertexShader, FragmentShader fragmentShader)
    {
        _shaderProgram.AttachShader(vertexShader);
        _shaderProgram.AttachShader(fragmentShader);
        _shaderProgram.Link();

        vertexShader.DestroyInContext(_glContext);
        fragmentShader.DestroyInContext(_glContext);
    }

    public void Push() => _glContext.UseProgram(_shaderProgram.ProgramObject);

    public void Pop() => _glContext.UseProgram(0U);
}