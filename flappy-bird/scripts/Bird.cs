using Godot;
using System;

namespace FlappyBird;

public partial class Bird : CharacterBody2D
{
    [Signal]
    public delegate void PassedEventHandler();
    [Signal]
    public delegate void DeadEventHandler();

    [Export]
    float mass = 0.6f;
    [Export]
    float jumpForce = 250f;

    AnimatedSprite2D animatedSprite;
    AudioStreamPlayer2D jumpSfxPlayer;
    AudioStreamPlayer2D passSfxPlayer;
    AudioStreamPlayer2D hitSfxPlayer;
    AudioStreamPlayer2D falldownSfxPlayer;

    float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public bool IsAlive
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        IsAlive = true;

        if (mass < 0f)
            mass = 0f;
        if (jumpForce < 0f)
            jumpForce = 0f;
        
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite.FlipV = false;

        jumpSfxPlayer = GetNode<AudioStreamPlayer2D>("JumpSfxPlayer");
        passSfxPlayer = GetNode<AudioStreamPlayer2D>("PassSfxPlayer");
        hitSfxPlayer = GetNode<AudioStreamPlayer2D>("HitSfxPlayer");
        falldownSfxPlayer = GetNode<AudioStreamPlayer2D>("FalldownSfxPlayer");
    }

    public override void _PhysicsProcess(double delta)
    {
        ApplyGravity((float)delta);
        HandleJump();
        MoveAndSlide();
        ApplyDead();
    }

    public void HandleJump()
    {
        if (IsAlive && Input.IsActionJustPressed("jump"))
        {
            var v = Velocity;
            // 높은 곳에서 떨어질 수록 중력 가속도 때문에 추락속도가 올라가기 때문에
            // 속도값을 -해주지 않고, 완전히 초기화 시켜버린다.
            v.Y = -jumpForce;
            Velocity = v;
            jumpSfxPlayer.Play();
        }
    }

    void ApplyGravity(float delta)
    {
        var v = Velocity;
        if (!IsOnFloor())
            v.Y += gravity * mass * delta;
        Velocity = v;
    }

    public void ApplyPass()
    {
        if (!IsAlive)
            return;
        passSfxPlayer.Play();
        EmitSignal(nameof(Passed));
    }

    void ApplyDead()
    {
        if (IsAlive)
        {
            // 벽에 부딛혀 사망
            if (IsOnWallOnly())
            {
                IsAlive = false;
                hitSfxPlayer.Play();
                if (!IsOnFloor())
                    falldownSfxPlayer.Play();
            }
            // 바닥이나 천장에 부딛혀 사망
            else if (IsOnFloorOnly() || IsOnCeilingOnly())
            {
                IsAlive = false;
                hitSfxPlayer.Play();
            }
            
            if (!IsAlive)
            {
                animatedSprite.Stop();
                animatedSprite.FlipV = true;
                EmitSignal(nameof(Dead));
            }
        }
    }
}
