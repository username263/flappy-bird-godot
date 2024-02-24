using Godot;
using System;

namespace FlappyBird;

public partial class BirdPortrait : Control
{
    [Export]
    ColorRect selcted;
    [Export]
    TextureRect birdTexture;
    [Export]
    Label coinLabel;
    [Export]
    Button button;

    public BirdTemplate Template
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        button.ButtonDown += () =>
        {
            var index = BirdTemplateGroup.Instance.IndexOf(Template);
            
            // 이미 선택된 새
            if (index == UserData.Instance.CurrentBirdIndex)
                return;

            if (UserData.Instance.HasBird(index))
            {
                UserData.Instance.CurrentBirdIndex = (short)index;
                return;
            }

            // 구매 실패
            if (!UserData.Instance.BuyBird(index))
                return;

            // 구매 성공하면 자동선택
            UserData.Instance.CurrentBirdIndex = (short)index;
        };

        UserData.Instance.BirdIndexChanged += Refresh;
    }

    public override void _ExitTree()
    {
        UserData.Instance.BirdIndexChanged -= Refresh;
    }

    public void Setup(BirdTemplate template)
    {
        Template = template;
        Refresh();
    }

    public void Refresh()
    {
        if (!Visible)
            return;

        var index = BirdTemplateGroup.Instance.IndexOf(Template);
        birdTexture.Texture = Template.Texture;
        birdTexture.ResetSize();
        coinLabel.Text = Template.Coin.ToString();

        coinLabel.Hide();
        selcted.Hide();
        
        if (index == UserData.Instance.CurrentBirdIndex)
        {
            selcted.Show();
            return;
        }
        
        if (!UserData.Instance.HasBird(index))
            coinLabel.Show();
    }
}
