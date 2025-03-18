using Godot;

namespace fpstutorial;

public partial class Enemy : CharacterBody3D
{
    [Export] private float _moveSpeed = 16;
    [Export] private PackedScene _bullet;

    private NavigationAgent3D _navAgent;
    private Timer _shootTimer;

    public override void _Ready()
    {
        _navAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        _shootTimer = GetNode<Timer>("ShootTimer");

        // handle shooting
        _shootTimer.Timeout += () =>
        {
            Bullet bulletInstance = (Bullet)_bullet.Instantiate();
            bulletInstance.Initialize(GetParent(), GlobalPosition, Player.PlayerInstance.GlobalPosition - GlobalPosition, false);
        };
    }

    public override void _PhysicsProcess(double deltaTime)
    {
        // handle walking
        _navAgent.TargetPosition = Player.PlayerInstance.GlobalPosition;
        Velocity = (_navAgent.GetNextPathPosition() - GlobalPosition).Normalized() * _moveSpeed;

        // handle gravity
        Velocity += Vector3.Down * 9.8f * (float)deltaTime;

        // move
        MoveAndSlide();
    }
}
