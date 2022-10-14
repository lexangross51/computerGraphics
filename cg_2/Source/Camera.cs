namespace cg_2.Source;

public enum TranslateDirection : byte
{
    Left,
    Right,
    Back,
    Forward
}

public class Camera
{
    private float _yaw, _pitch;
    private float _lastX, _lastY;
    private Vector3 _direction;

    public Vector3 Position { get; private set; }
    public Vector3 View { get; private set; }
    public Vector3 Up { get; private set; }
    public Vector3 Right { get; private set; }
    public Vector3 WorldUp { get; private set; }
    public float Sensitivity { get; set; }
    public float Speed { get; set; }

    public Camera()
    {
        Position = new Vector3(0.0f, 0.0f, 0.0f);
        View = new Vector3(0.2f, 0.2f, 0.2f);
        WorldUp = new Vector3(0.0f, 1.0f, 0.0f);
        Up = new Vector3(0.0f, 1.0f, 0.0f);
        _direction = new();
        Sensitivity = 0.2f;
        Speed = 0.01f;
        _yaw = 0.0f;
        _pitch = 0.0f;
        _lastX = 0.0f;
        _lastY = 0.0f;
    }

    public Camera(Vector3 position, Vector3 view, Vector3 worldUp) : this() =>
        (Position, View, WorldUp) = (position, view, worldUp);

    public void CameraPosition(Vector3 position, Vector3 view, Vector3 worldUp) =>
        (Position, View, WorldUp) = (position, view, worldUp);

    public void LookAt(float xPos, float yPos)
    {
        if (xPos == _lastX && yPos == _lastY) return;

        float offsetX = xPos - _lastX;
        float offsetY = _lastY - yPos;

        _lastX = xPos;
        _lastY = yPos;

        _yaw += offsetX * Sensitivity;
        _pitch += offsetY * Sensitivity;

        if (_pitch > 89.0f) _pitch = 89.0f;
        if (_pitch < -89.0f) _pitch = -89.0f;

        float radYaw = (float)(_yaw * Math.PI / 180.0f);
        float radPitch = (float)(_pitch * Math.PI / 180.0f);

        float cosYaw = (float)Math.Cos(radYaw);
        float sinYaw = (float)Math.Sin(radYaw);
        float cosPitch = (float)Math.Cos(radPitch);
        float sinPitch = (float)Math.Sin(radPitch);

        // Считаем направление
        _direction.X = cosYaw * cosPitch;
        _direction.Y = sinPitch;
        _direction.Z = sinYaw * cosPitch;
        _direction.Normalize();

        // После этого посчитаем вектор правой и верхней осей
        Right = _direction.Cross(WorldUp).Normalize();
        Up = Right.Cross(_direction).Normalize();
        View = Position + _direction;
    }

    public void Translate(TranslateDirection direction)
    {
        if (direction == TranslateDirection.Forward)
        {
            Position += _direction * Speed;
        }
        if (direction == TranslateDirection.Back)
        {
            Position -= _direction * Speed;
        }
        if (direction == TranslateDirection.Right)
        {
            Position += Right * Speed;
            View += Right * Speed;
        }
        if (direction == TranslateDirection.Left)
        {
            Position -= Right * Speed;
            View -= Right * Speed;
        }
    }
}