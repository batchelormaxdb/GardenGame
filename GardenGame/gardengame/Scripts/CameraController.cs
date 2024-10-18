using Godot;
using System;

public partial class CameraController : Node3D
{
    [Export] 
    public float RotationSpeed = 0.01f;
    [Export] 
    public float ZoomSpeed = 2f;
    [Export] 
    public float MinZoomDistance = 2f;
    [Export] 
    public float MaxZoomDistance = 20f;
    [Export]
    private Camera3D Camera;
    private bool IsRotating = false;
    private float Speed = 20f;
    public override void  _Ready()
    {
        Camera = this.FindChild("Camera3D") as Camera3D;
    }
    public override void _Process(double delta)
    {
        Vector2 inputVector = Input.GetVector("MoveLeft", "MoveRight", "MoveForward", "MoveBackward");

        Vector3 translatedInput = new Vector3(inputVector.X, 0, inputVector.Y);

        translatedInput = translatedInput.Rotated(Vector3.Up, Rotation.Y);

        Position += translatedInput * (float)delta * Speed;

        //Force camera to always look at this cameraArm node
        Camera.LookAt(this.GlobalPosition);
    }
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            //Only trigger rotation enable bool when left mouse button is being held
            IsRotating = mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed;

            // Handle scroll wheel zooming
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
                ZoomCamera(-ZoomSpeed);
            else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
                ZoomCamera(ZoomSpeed);
        }

        // Handle mouse motion for camera rotation
        if (@event is InputEventMouseMotion mouseMotionEvent && IsRotating)
        {
            Vector2 mouseDelta = mouseMotionEvent.Relative;
            RotateY(-mouseDelta.X * RotationSpeed);
        }
    }

    private void ZoomCamera(float zoomAmount)
    {
        Console.WriteLine("Zooming");

        Vector3 NewPosition = Camera.GlobalPosition.MoveToward(GlobalPosition, zoomAmount);

        float CameraDistance = NewPosition.DistanceTo(GlobalPosition);

        if (CameraDistance >= MinZoomDistance && CameraDistance <= MaxZoomDistance)
        {
            Camera.GlobalPosition = NewPosition;
        }
        
    }
}
