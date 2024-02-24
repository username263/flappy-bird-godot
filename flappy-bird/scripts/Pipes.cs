using Godot;
using System;

namespace FlappyBird;

public partial class Pipes : Node2D
{
    Node2D topPipe;
    Node2D bottomPipe;
    Area2D passArea;


    public override void _Ready()
    {
        topPipe = GetNode<Node2D>("TopPipe");
        bottomPipe = GetNode<Node2D>("BottomPipe");
        passArea = GetNode<Area2D>("PassArea");
        passArea.BodyEntered += body =>
        {
            if (body is Bird bird)
                bird.ApplyPass();
        };
    }

    /// <summary>
    /// 파이프 사이 간격을 조절한다.
    /// 나중에 난이도에 맞춰 파이프가 좁아지는 처리하기 위해 만들어둠
    /// </summary>
    public void SetPipesInterval(float halfInterval)
    {
        topPipe.Position = new(0, -halfInterval);
        bottomPipe.Position = new(0, halfInterval);
    }
}
