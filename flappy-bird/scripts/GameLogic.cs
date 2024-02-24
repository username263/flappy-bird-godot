using Godot;
using System;

namespace FlappyBird;

public partial class GameLogic : Node2D
{
    [Export]
    Marker2D startPoint;
    [Export]
    PipesManager pipesManager;
    [Export]
    Background background;
    [Export]
    float gameSpeed = 10f;
    [Export]
    Label scoreLabel;
    [Export]
    Control gameOver;
    [Export]
    Button retryButton;
    [Export]
    Button menuButton;

    int score;

    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreLabel.Text = score.ToString();
        }
    }

    public override void _Ready()
    {
        Score = 0;
        gameOver.Hide();

        var template = BirdTemplateGroup.Instance[UserData.Instance.CurrentBirdIndex];
        var bird = template.SpawnBird();
        AddChild(bird);
        bird.GlobalPosition = startPoint.GlobalPosition;
        bird.GlobalRotation = startPoint.GlobalRotation;
        bird.Passed += () =>
        {
            ++Score;
        };
        bird.Dead += async () =>
        {
            background.ScrollSpeed = 0f;
            pipesManager.IsEnable = false;
            
            UserData.Instance.UpdateScore(Score);

            await ToSignal(GetTree().CreateTimer(1), Timer.SignalName.Timeout);
            gameOver.Show();
        };

        background.ScrollSpeed = gameSpeed;
        pipesManager.MoveSpeed = gameSpeed;
        pipesManager.IsEnable = true;

        retryButton.Pressed += () =>
        {
            GetTree().ReloadCurrentScene();
        };
        menuButton.Pressed += () =>
        {
            GetTree().ChangeSceneToFile("res://flappy-bird/scenes/Menu.tscn");
        };
    }
}
