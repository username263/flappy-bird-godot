using Godot;
using Godot.Collections;
using System;

namespace FlappyBird;

public partial class ShopPage : Control
{
    [Export]
    Label coinLabel;
    [Export]
    Array<BirdPortrait> birdPortraits;
    [Export]
    Button backButton;


    public override void _Ready()
    {
        OnCoinChanged();
        UserData.Instance.CoinChanged += OnCoinChanged;

        var birdTempGroup = BirdTemplateGroup.Instance;
        for (int i = 0; i < birdPortraits.Count; i++)
        {
            if (i >= birdTempGroup.Count)
            {
                birdPortraits[i].Hide();
                continue;
            }
            birdPortraits[i].Show();
            birdPortraits[i].Setup(birdTempGroup[i]);
        }

        backButton.Pressed += () =>
        {
            MenuLogic.Instance.ShowMainPage();
        };
    }

    public override void _ExitTree()
    {
        UserData.Instance.CoinChanged -= OnCoinChanged;
    }

    void OnCoinChanged()
    {
        coinLabel.Text = UserData.Instance.Coin.ToString();
    }
}
