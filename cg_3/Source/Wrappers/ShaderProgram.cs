namespace cg_3.Source.Wrappers;

public static class ShadersResource
{
    public const string VertexShader = """
                                        #version 330 core
                                        layout (location = 0) in vec2 position;
                                        uniform mat4 model;
                                        uniform mat4 view;
                                        uniform mat4 projection;
                                        void main() {
                                            gl_PointSize = 5;
                                            gl_Position = projection * view * model * vec4(position, 0.0f, 1.0f);
                                        }
                                        """;

    public const string FragmentShader = """ 
                                        #version 330 core 
                                        uniform vec3 color;
                                        out vec4 FragColor;
                                        void main() {
                                        FragColor = vec4(color, 1.0);
                                        } 
                                        """;
}

public class ShaderProgram : IDisposable
{
    public int Handle { get; }
    public Dictionary<string, int> UniformLocation { get; } = new();

    public ShaderProgram() => Handle = GL.CreateProgram();

    public void Use() => GL.UseProgram(Handle);

    public void Initialize(string vertexShaderPath, string fragmentShaderPath, string? geometryShaderPath = null)
    {
        string shaderSource;
        int? geometryShader = null;

        var sr = new StreamReader(vertexShaderPath);
        using (sr)
        {
            shaderSource = sr.ReadToEnd();
        }

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, shaderSource);

        sr = new(fragmentShaderPath);
        using (sr)
        {
            shaderSource = sr.ReadToEnd();
        }

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);

        if (geometryShaderPath is not null)
        {
            sr = new(geometryShaderPath);
            using (sr)
            {
                shaderSource = sr.ReadToEnd();
            }

            geometryShader = GL.CreateShader(ShaderType.GeometryShader);
            GL.ShaderSource(geometryShader.Value, shaderSource);
            CompileShader(geometryShader.Value);
            GL.AttachShader(Handle, geometryShader.Value);
        }

        CompileShader(vertexShader);
        CompileShader(fragmentShader);

        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        LinkProgram(Handle);

        if (geometryShader.HasValue)
        {
            GL.DetachShader(Handle, geometryShader.Value);
            GL.DeleteShader(geometryShader.Value);
        }

        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        for (int i = 0; i < numberOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);
            var location = GL.GetUniformLocation(Handle, key);
            UniformLocation.Add(key, location);
        }
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

        if (code == (int)All.True) return;

        var infoLog = GL.GetShaderInfoLog(shader);
        throw new($"Error occurred whilst compiling Shader({shader}).\n\n{infoLog}");
    }

    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);
        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);

        if (code == (int)All.True) return;

        throw new($"Error occurred whilst linking Program({program})");
    }

    public int GetAttributeLocation(string name) => GL.GetAttribLocation(Handle, name);

    public void SetUniform(string name, int value)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.Uniform1(UniformLocation[name], value);
    }

    public void SetUniform(string name, float value)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.Uniform1(UniformLocation[name], value);
    }

    public void SetUniform(string name, float x, float y, float z)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.Uniform3(UniformLocation[name], x, y, z);
    }

    public void SetUniform(string name, float x, float y, float z, float w)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.Uniform4(UniformLocation[name], x, y, z, w);
    }

    public void SetUniform(string name, Vector3 value)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.Uniform3(UniformLocation[name], value);
    }

    public void SetUniform(string name, Color4 color)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.Uniform3(UniformLocation[name], new(color.R, color.G, color.B));
    }

    public void SetUniform(string name, Matrix4 value)
    {
        if (!UniformLocation.ContainsKey(name)) throw new($"{name} uniform not found on shader");
        GL.UniformMatrix4(UniformLocation[name], false, ref value);
    }

    public void Dispose() => GL.DeleteProgram(Handle);

    public static ShaderProgram StandartProgram()
    {
        ShaderProgram shaderProgram = new();
        var shaderSource = ShadersResource.VertexShader;

        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, shaderSource);

        shaderSource = ShadersResource.FragmentShader;

        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, shaderSource);

        CompileShader(vertexShader);
        CompileShader(fragmentShader);

        GL.AttachShader(shaderProgram.Handle, vertexShader);
        GL.AttachShader(shaderProgram.Handle, fragmentShader);

        LinkProgram(shaderProgram.Handle);

        GL.DetachShader(shaderProgram.Handle, vertexShader);
        GL.DetachShader(shaderProgram.Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);

        GL.GetProgram(shaderProgram.Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);

        for (int i = 0; i < numberOfUniforms; i++)
        {
            var key = GL.GetActiveUniform(shaderProgram.Handle, i, out _, out _);
            var location = GL.GetUniformLocation(shaderProgram.Handle, key);
            shaderProgram.UniformLocation.Add(key, location);
        }

        return shaderProgram;
    }
}