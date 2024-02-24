using Godot;
using System;

namespace FlappyBird;

public partial class Background : ParallaxBackground
{
    public float ScrollSpeed
    {
        get;
        set;
    }

    public override void _Process(double delta)
    {
        var offset = ScrollOffset;
        offset.X -= (float)delta * ScrollSpeed;
        ScrollOffset = offset;
    }
}
