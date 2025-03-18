using Godot;

namespace fpstutorial;

public partial class Bullet : Area3D
{
    [Export] private float _moveSpeed = 48;

    private Vector3 _direction;
    private bool _ownedByPlayer;

    public override void _Ready()
    {
        BodyEntered += body =>
        {
            if (_ownedByPlayer && body is not Player)
            {
                if (body is Enemy enemy)
                {
                    enemy.QueueFree();
                }
                else
                {
                    QueueFree();
                }
            }
            else if (body is not Enemy)
            {
                if (body is Player player)
                {
                    // reload level
                }
                else
                {
                    QueueFree();
                }
            }
        };
    }

    public override void _PhysicsProcess(double deltaTime)
    {
        GlobalPosition += _direction.Normalized() * _moveSpeed * (float)deltaTime;
    }

    public void Initialize(Node parent, Vector3 pos, Vector3 direction, bool ownedByPlayer)
    {
        parent.AddChild(this);
        GlobalPosition = pos;
        _direction = direction;
        _ownedByPlayer = ownedByPlayer;
        LookAt(GlobalPosition + _direction);
    }
}
