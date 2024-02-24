using Godot;
using System;

namespace FlappyBird;

public partial class MainPage : Control
{
    [Export]
    Button playButton;
    [Export]
    Button shopButton;
    [Export]
    Button quitButton;

    public override void _Ready()
    {
        playButton.Pressed += () =>
        {
            MenuLogic.Instance.GotoGame();
        };
        shopButton.Pressed += () =>
        {
            MenuLogic.Instance.ShowShopPage();
        };
        quitButton.Pressed += () =>
        {
            GetTree().Quit();
        };
    }
}
