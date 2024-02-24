using Godot;
using System;

namespace FlappyBird;

public partial class MenuLogic : Node2D
{
    [Export]
    Background background;
    [Export]
    Control mainPage;
    [Export]
    Control shopPage;

    public static MenuLogic Instance
    {
        get;
        private set;
    }

    public MenuLogic()
    {
        Instance = this;
    }

    public override void _Ready()
    {
        background.ScrollSpeed = 50;
        UserData.Instance.BirdIndexChanged += OnBirdIndexChanged;
        ShowMainPage();
    }

    public override void _ExitTree()
    {
        UserData.Instance.BirdIndexChanged -= OnBirdIndexChanged;
    }

    public void GotoGame()
    {
        GetTree().ChangeSceneToFile("res://flappy-bird/scenes/World.tscn");
    }

    public void ShowMainPage()
    {
        shopPage.Hide();
        mainPage.Show();
    }

    public void ShowShopPage()
    {
        mainPage.Hide();
        shopPage.Show();
    }

    void OnBirdIndexChanged()
    {
        
    }
}
