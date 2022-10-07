namespace cg_2.Source;

public class Camera
{
    private float _yaw, _pitch;
    private float _lastX, _lastY;

    public Vector3 Position { get; set; }
    public Vector3 View { get; set; }
    public Vector3 Up { get; set; }
    public Vector3 Right { get; set; }
    public float Sensitivity { get; set; }

    public Camera()
    {
        Position = new Vector3(0.0f, 0.0f, 3.0f);
        View = new Vector3(0.0f, 0.0f, -1.0f);
        Up = new Vector3(0.0f, 1.0f, 0.0f);
        Sensitivity = 0.1f;
    }

    public Camera(Vector3 position, Vector3 view, Vector3 up)
    {
        (Position, View, Up) = (position, view, up);
        _yaw = 0.0f;
        _pitch = 0.0f;
        _lastX = 0.0f;
        _lastY = 0.0f;
        Sensitivity = 0.01f;
    }

    public void CameraPosition(Vector3 position, Vector3 view, Vector3 up) =>
        (Position, View, Up) = (position, view, up);

    public void LookAt(float xPos, float yPos)
    {
        float offsetX = xPos - _lastX;
        float offsetY = _lastY - yPos;

        _lastX = xPos;
        _lastY = yPos;

        offsetX *= Sensitivity;
        offsetY *= Sensitivity;

        _yaw += offsetX;
        _pitch += offsetY;

        if (_pitch > 89.0f) _pitch = 89.0f;
        if (_pitch < -89.0f) _pitch = -89.0f;

        float radYaw = (float)(_yaw * Math.PI / 180.0f);
        float radPitch = (float)(_pitch * Math.PI / 180.0f);

        float cosYaw = (float)Math.Cos(radYaw);
        float sinYaw = (float)Math.Sin(radYaw);
        float cosPitch = (float)Math.Cos(radPitch);
        float sinPitch = (float)Math.Sin(radPitch);

        View = new(cosYaw * cosPitch, sinPitch, sinYaw * cosPitch);
        View.Normalize();

        Right = View.Cross(Up).Normalize();
        Up = Right.Cross(View).Normalize();
    }
}