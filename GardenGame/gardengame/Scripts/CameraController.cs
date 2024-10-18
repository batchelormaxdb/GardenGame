using Godot;
using System;

public partial class CameraController : Node3D
{
    [Export] public float RotationSpeed = 0.01f; // Rotation speed factor
    public int test;
    [Export] public float ZoomSpeed = 2f; // Speed for zooming in/out
    [Export] public float MinZoomDistance = 2f; // Minimum zoom distance
    [Export] public float MaxZoomDistance = 20f; // Maximum zoom distance
    private Camera3D Camera;
    private bool IsRotating = false;
    private Vector2 LastMousePos;
    private float Speed = 20f;
    public override void  _Ready()
    {
        Camera = new Camera3D();
        Camera.Position = new Vector3(0, 2.3f, 2.3f);
        Camera.Rotation = new Vector3(-45,0,0);
    }
    public override void _Process(double delta)
    {
        Vector3 velocity = Vector3.Zero;
        InputEventMouseMotion mouseMovement = new InputEventMouseMotion();

        if (Input.IsActionPressed("MoveLeft"))
        {
            velocity.X -= 0.5f;
        }
        if (Input.IsActionPressed("MoveRight"))
        {
            velocity.X += 0.5f;
        }
        if (Input.IsActionPressed("MoveForward"))
        {
            velocity.Z -= 0.5f;
        }
        if (Input.IsActionPressed("MoveBackward"))
        {
            velocity.Z += 0.5f;
        }
        Position += velocity * (float)delta * Speed;
    }
        public override void _Input(InputEvent @event)
    {
        // Handle mouse button input for rotation
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            // Check if the left mouse button is pressed or released
            if (mouseButtonEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseButtonEvent.Pressed)
                {
                    IsRotating = true;
                    LastMousePos = mouseButtonEvent.Position;
                }
                else
                {
                    IsRotating = false;
                }
            }
            // Handle scroll wheel zooming
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                ZoomCamera(-ZoomSpeed);
            }
            else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
            {
                ZoomCamera(ZoomSpeed);
            }
        }

        // Handle mouse motion for camera rotation
        if (@event is InputEventMouseMotion mouseMotionEvent && IsRotating)
        {
            Vector2 mouseDelta = mouseMotionEvent.Relative;
            RotateY(-mouseDelta.X * RotationSpeed); // Rotate around the Y axis (yaw)
        }
    }

    private void ZoomCamera(float zoomAmount)
    {
        // Get the forward direction (relative to the camera's current orientation)
        Vector3 forward = GlobalTransform.Basis.Z; // Z points forward

        // Move the camera along the forward direction
        Vector3 newPosition = Position + forward * zoomAmount;

        // Limit zoom distance
        float distanceToOrigin = newPosition.Length();
        if (distanceToOrigin >= MinZoomDistance && distanceToOrigin <= MaxZoomDistance)
        {
            Position = newPosition;
        }
    }
}
