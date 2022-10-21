namespace cg_2.Source.Camera;

public class DeltaTime
{
    private float _lastFrame;
    private float _deltaTime;
    private readonly Stopwatch _sw = new();

    public float Result => _deltaTime;

    public void Compute()
    {
        _sw.Stop();

        float currentFrame = _sw.ElapsedMilliseconds / 1000.0f;
        _deltaTime = currentFrame - _lastFrame;
        _lastFrame = currentFrame;

        _sw.Start();
    }
}

public enum CameraMovement
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

public class Camera
{
    private float _yaw;
    private float _pitch;
    private float _lastX, _lastY;
    private vec3 _worldUp;
    private vec3 _right;

    public bool FirstMouse { get; set; }
    public float Sensitivity { get; set; }
    public float Speed { get; set; }

    public vec3 Position { get; private set; }
    public vec3 Front { get; private set; }
    public vec3 Up { get; private set; }

    public Camera()
    {
        Position = new vec3(0.0f, 0.0f, 3.0f);
        Front = new vec3(0.0f, 0.0f, -1.0f);
        _worldUp = new vec3(0.0f, 1.0f, 0.0f);
        Up = new vec3(0.0f, 1.0f, 0.0f);
        Sensitivity = 0.1f;
        Speed = 6f;
        _yaw = -90.0f;
        _pitch = 0.0f;
        _lastX = _lastY = 0.0f;
        FirstMouse = true;

        UpdateVectors();
    }

    public void CameraPosition(vec3 position, vec3 front, vec3 up)
        => (Position, Front, _worldUp) = (position, front, up);

    public void LookAt(float xPos, float yPos)
    {
        if (Math.Abs(_lastX - xPos) < 1E-05 && Math.Abs(_lastY - yPos) < 1E-05) return;

        if (FirstMouse)
        {
            _lastX = xPos;
            _lastY = yPos;
            FirstMouse = false;
        }

        var offsetX = xPos - _lastX;
        var offsetY = _lastY - yPos;

        _lastX = xPos;
        _lastY = yPos;

        _yaw += offsetX * Sensitivity;
        _pitch += offsetY * Sensitivity;

        if (_pitch > 89.0f) _pitch = 89.0f;
        if (_pitch < -89.0f) _pitch = -89.0f;

        UpdateVectors();
    }

    public void Move(CameraMovement direction, float deltaTime)
    {
        var velocity = Speed * deltaTime;

        Position += direction switch
        {
            CameraMovement.Forward => Front * velocity,
            CameraMovement.Backward => -1.0f * (Front * velocity),
            CameraMovement.Right => _right * velocity,
            CameraMovement.Left => -1.0f * (_right * velocity),
            CameraMovement.Up => Up * velocity,
            CameraMovement.Down => -1.0f * Up * velocity,
            _ => throw new ArgumentOutOfRangeException(nameof(direction),
                $"Not expected direction value: {direction}")
        };
    }

    private void UpdateVectors()
    {
        var radYaw = glm.radians(_yaw);
        var radPitch = glm.radians(_pitch);

        var cosYaw = glm.cos(radYaw);
        var sinYaw = glm.sin(radYaw);
        var cosPitch = glm.cos(radPitch);
        var sinPitch = glm.sin(radPitch);

        vec3 front = new()
        {
            x = cosYaw * cosPitch,
            y = sinPitch,
            z = sinYaw * cosPitch
        };

        Front = glm.normalize(front);
        _right = glm.normalize(glm.cross(Front, _worldUp));
        Up = glm.normalize(glm.cross(_right, Front));
    }
}