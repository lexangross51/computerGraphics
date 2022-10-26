namespace cg_2.Source.Camera;

public enum CameraMovement
{
    Forward,
    Backward,
    Left,
    Right,
    Up,
    Down
}

public enum CameraMode
{
    Perspective,
    Orthographic
}

public class MainCamera
{
    private float _yaw;
    private float _pitch;
    private float _lastX, _lastY;
    private Vector3 _worldUp;
    private Vector3 _right;

    public bool FirstMouse { get; set; }
    public float Sensitivity { get; set; }
    public float Speed { get; set; }

    public Vector3 Position { get; private set; }
    public Vector3 Front { get; private set; }
    public Vector3 Up { get; private set; }
    public CameraMode CameraMode { get; set; }

    public MainCamera(CameraMode cameraMode)
    {
        Position = new Vector3(0.0f, 0.0f, 3.0f);
        Front = new Vector3(0.0f, 0.0f, -1.0f);
        _worldUp = new Vector3(0.0f, 1.0f, 0.0f);
        Up = new Vector3(0.0f, 1.0f, 0.0f);
        Sensitivity = 0.1f;
        Speed = 6f;
        _yaw = -90.0f;
        _pitch = 0.0f;
        _lastX = _lastY = 0.0f;
        FirstMouse = true;
        CameraMode = cameraMode;

        UpdateVectors();
    }

    public void CameraPosition(Vector3 position, Vector3 front, Vector3 up, CameraMode cameraMode)
        => (Position, Front, _worldUp, CameraMode) = (position, front, up, cameraMode);

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
        var radYaw = MathHelper.DegreesToRadians(_yaw);
        var radPitch = MathHelper.DegreesToRadians(_pitch);

        var cosYaw = (float)MathHelper.Cos(radYaw);
        var sinYaw = (float)MathHelper.Sin(radYaw);
        var cosPitch = (float)MathHelper.Cos(radPitch);
        var sinPitch = (float)MathHelper.Sin(radPitch);

        Vector3 front = new()
        {
            X = cosYaw * cosPitch,
            Y = sinPitch,
            Z = sinYaw * cosPitch
        };

        Front = Vector3.Normalize(front);
        _right = Vector3.Normalize(Vector3.Cross(Front, _worldUp));
        Up = Vector3.Normalize(Vector3.Cross(_right, Front));
    }
}