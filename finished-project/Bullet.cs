using Godot;

namespace fpstutorial;

public partial class Bullet : Area3D
{
    [Export] private float _moveSpeed = 128;

    private Vector3 _direction;

    public override void _Ready()
    {
        BodyEntered += body =>
        {
            if (body is Player player)
            {
            }
        };
    }

    public override void _PhysicsProcess(double deltaTime)
    {
        GlobalPosition += _direction.Normalized() * _moveSpeed * (float)deltaTime;
    }

    public void Initialize(Vector3 pos, Vector3 direction)
    {
        GlobalPosition = pos;
        _direction = direction;
        LookAt(GlobalPosition + _direction);
    }
}
