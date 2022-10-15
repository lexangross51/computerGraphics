namespace cg_2.Source.Camera;

public enum TranslateDirection : byte
{
    Left,
    Right,
    Back,
    Forward
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
        Sensitivity = 0.2f;
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

    public void Move(TranslateDirection direction, double deltaTime)
    {
        var velocity = (float)(Speed * deltaTime);

        switch (direction)
        {
            case TranslateDirection.Forward:
                Position += Front * velocity;
                break;
            case TranslateDirection.Back:
                Position -= Front * velocity;
                break;
            case TranslateDirection.Right:
                Position += _right * velocity;
                break;
            case TranslateDirection.Left:
                Position -= _right * velocity;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
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