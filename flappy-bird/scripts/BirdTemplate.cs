using Godot;
using System;

namespace FlappyBird;

public partial class BirdTemplate : Resource
{
    [Export]
    Texture2D texture;
    [Export]
    PackedScene birdScene;
    [Export]
    int coin;

    public Texture2D Texture => texture;

    public int Coin => coin;

    public Bird SpawnBird()
    {
        return birdScene.Instantiate<Bird>();
    }
}
