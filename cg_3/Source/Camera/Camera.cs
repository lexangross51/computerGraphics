﻿using OpenTK.Mathematics;

namespace cg_3.Source.Camera;

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
    private readonly Vector3 _worldUp;
    private Vector3 _right;
    private float _fov = MathHelper.PiOver2;

    public bool FirstMouse { get; set; }
    public float Sensitivity { get; set; }
    public float Speed { get; set; }
    public float AspectRatio { get; set; }

    public Vector3 Position { get; private set; }
    public Vector3 Front { get; private set; }
    public Vector3 Up { get; private set; }
    public CameraMode CameraMode { get; set; }

    public float Fov
    {
        get => MathHelper.RadiansToDegrees(_fov);
        set
        {
            var angle = MathHelper.Clamp(value, 1.0f, 90.0f);
            _fov = MathHelper.DegreesToRadians(angle);
        }
    }

    public MainCamera(CameraMode cameraMode)
    {
        Position = new(0.0f, 0.0f, 3.0f);
        Front = new(0.0f, 0.0f, -1.0f);
        _worldUp = new(0.0f, 1.0f, 0.0f);
        Up = new(0.0f, 1.0f, 0.0f);
        Sensitivity = 0.1f;
        Speed = 0.01f;
        _yaw = -90.0f;
        _pitch = 0.0f;
        _lastX = _lastY = 0.0f;
        FirstMouse = true;
        CameraMode = cameraMode;

        UpdateVectors();
    }

    public MainCamera(Vector3 position, Vector3 front, Vector3 up, CameraMode cameraMode)
        => (Position, Front, _worldUp, CameraMode) = (position, front, up, cameraMode);

    public Matrix4 GetViewMatrix() => Matrix4.LookAt(Position, Position + Front, Up);

    public Matrix4 GetProjectionMatrix() => CameraMode is CameraMode.Perspective
        ? Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 100.0f)
        : Matrix4.CreateOrthographic(1920.0f, 1080.0f, 0.01f, 100.0f); // took render size

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