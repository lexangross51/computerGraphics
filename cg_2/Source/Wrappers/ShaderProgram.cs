namespace cg_2.Source.Wrappers;

public class ShaderProgram : IDisposable
{
    private readonly Dictionary<string, int> _uniformLocations = new();
    public int Handle { get; }

    public ShaderProgram() => Handle = GL.CreateProgram();

    public void Use() => GL.UseProgram(Handle);

    public void Initialize(string vertexShaderPath, string fragmentShaderPath)
    {
        string shaderSource;

        var sr = new StreamReader(vertexShaderPath);
        using (sr)
        {
            shaderSource = sr.ReadToEnd();
        }

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, shaderSource);

        sr = new StreamReader(fragmentShaderPath);
        using (sr)
        {
            shaderSource = sr.ReadToEnd();
        }

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);

        CompileShader(vertexShader);
        CompileShader(fragmentShader);

        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        LinkProgram(Handle);

        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        for (int i = 0; i < numberOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);
            var location = GL.GetUniformLocation(Handle, key);
            _uniformLocations.Add(key, location);
        }
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

        if (code == (int)All.True) return;

        var infoLog = GL.GetShaderInfoLog(shader);
        throw new Exception($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
    }

    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);

        if (code == (int)All.True) return;

        throw new Exception($"Error occurred whilst linking Program({program})");
    }

    public int GetAttributeLocation(string name) => GL.GetAttribLocation(Handle, name);

    public void SetUniform(string name, int value)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, float value)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.Uniform1(_uniformLocations[name], value);
    }

    public void SetUniform(string name, float x, float y, float z)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.Uniform3(_uniformLocations[name], x, y, z);
    }

    public void SetUniform(string name, float x, float y, float z, float w)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.Uniform4(_uniformLocations[name], x, y, z, w);
    }

    public void SetUniform(string name, Vector3 value)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.Uniform3(_uniformLocations[name], value);
    }

    public void SetUniform(string name, Color4 color)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.Uniform3(_uniformLocations[name], new(color.R, color.G, color.B));
    }

    public void SetUniform(string name, Matrix4 value)
    {
        if (!_uniformLocations.ContainsKey(name)) throw new Exception($"{name} uniform not found on shader");
        GL.UniformMatrix4(_uniformLocations[name], false, ref value);
    }

    public void Dispose() => GL.DeleteProgram(Handle);
}