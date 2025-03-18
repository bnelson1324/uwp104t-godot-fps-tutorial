using Godot;

namespace fpstutorial;

public partial class Player : CharacterBody3D
{
    private const float Sensitivity = 0.002f;

    public static Player PlayerInstance;

    [Export] private float _moveSpeed = 32;
    [Export] private float _jumpSpeed = 12;
    [Export] private PackedScene _bullet;

    private Camera3D _camera;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _camera = GetNode<Camera3D>("Camera3D");
        PlayerInstance = this;
    }

    public override void _PhysicsProcess(double deltaTime)
    {
        // handle walking
        Vector2 moveInput = Input.GetVector("walk_left", "walk_right", "walk_forward", "walk_backward");
        Vector3 moveDir = ((_camera.Basis * new Vector3(moveInput.X, 0, moveInput.Y)) with { Y = 0 }).Normalized();
        Velocity = _moveSpeed * moveDir + Vector3.Up * Velocity.Y;

        // handle jumping
        if (Input.IsActionPressed("jump") && IsOnFloor())
            Velocity = Velocity with { Y = 0 } + Vector3.Up * _jumpSpeed;

        // handle gravity
        Velocity += Vector3.Down * 24f * (float)deltaTime;

        // move
        MoveAndSlide();

        // handle shooting
        if (Input.IsActionJustPressed("shoot"))
        {
            Bullet bulletInstance = (Bullet)_bullet.Instantiate();
            bulletInstance.Initialize(GetParent(), _camera.GlobalPosition, -_camera.Basis.Z, true);
        }
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        // handle looking
        if (@event is InputEventMouseMotion eventMouseMotion)
        {
            float xMove = eventMouseMotion.Relative.X;
            float yMove = eventMouseMotion.Relative.Y;
            _camera.GlobalRotation += new Vector3(-yMove, -xMove, 0) * Sensitivity;
        }
    }
}
