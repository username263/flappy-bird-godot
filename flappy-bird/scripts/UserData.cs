using Godot;
using Godot.Collections;
using System;

namespace FlappyBird;

public partial class UserData : Node
{
    [Signal]
    public delegate void CoinChangedEventHandler();
    [Signal]
    public delegate void ScoreChangedEventHandler();
    [Signal]
    public delegate void BirdIndexChangedEventHandler();
    [Signal]
    public delegate void HasBirdsChangedEventHandler();

    const string savePath = "user://UserData.save";

    long coin;
    short curBirdIdx;
    Array<bool> hasBirds;
    long totalScore;
    int bestScore;
    int lastScore;

    public long Coin => coin;

    public short CurrentBirdIndex
    {
        get => curBirdIdx;
        set
        {
            if (!HasBird(value))
                return;
            curBirdIdx = value;
            EmitSignal(nameof(BirdIndexChanged));
            Save();
        }
    }

    public long TotalScore => totalScore;

    public int BestScore => bestScore;

    public int LastScore => lastScore;

    public static UserData Instance
    {
        get;
        private set;
    }

    public override void _Ready()
    {
        //Instance = GetNode<UserData>($"/root/{nameof(UserData)}");
        Instance = this;
        Load();
    }

    public void UpdateScore(int newScore)
    {
        if (newScore < 0)
            return;
        
        lastScore = newScore;
        totalScore += newScore;
        bestScore = Math.Max(newScore, bestScore);
        EmitSignal(nameof(ScoreChanged));

        coin += newScore;
        EmitSignal(nameof(CoinChanged));

        Save();
    }

    public bool HasBird(int birdIndex)
    {
        if (birdIndex >= hasBirds.Count)
            return false;
        if (!hasBirds[birdIndex])
            return false;
        return true;
    }

    public bool BuyBird(int birdIndex)
    {
        if (HasBird(birdIndex))
            return false;

        var birdTempGroup = BirdTemplateGroup.Instance;
        if (birdIndex >= birdTempGroup.Count)
            return false;
        var template = birdTempGroup[birdIndex];
        if (template == null)
            return false;
        if (Coin < template.Coin)
            return false;
        
        coin -= template.Coin;
        EmitSignal(nameof(CoinChanged));
        
        for (int i = hasBirds.Count; i < birdTempGroup.Count; i++)
            hasBirds.Add(false);
        hasBirds[birdIndex] = true;
        EmitSignal(nameof(HasBirdsChanged));

        Save();

        return true;
    }

    public void Save()
    {
        var data = new Dictionary<string, Variant>();
        data[nameof(coin)] = coin;
        data[nameof(curBirdIdx)] = curBirdIdx;
        data[nameof(hasBirds)] = hasBirds;
        data[nameof(totalScore)] = totalScore;
        data[nameof(bestScore)] = bestScore;
        data[nameof(lastScore)] = lastScore;
        var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Write);
        file.StoreString(data.ToString());
        file.Close();
    }

    public void Load()
    {
        try
        {
            var file = FileAccess.Open(savePath, FileAccess.ModeFlags.Read);
            if (file == null)
				GD.PrintErr(FileAccess.GetOpenError());
            var data = Json.ParseString(file.GetAsText()).AsGodotDictionary<string, Variant>();
            file.Close();

            data.TryGetValue(nameof(coin), out var v);
            coin = v.AsInt64();

            data.TryGetValue(nameof(curBirdIdx), out v);
            curBirdIdx = v.AsInt16();

            data.TryGetValue(nameof(hasBirds), out v);
            hasBirds = v.AsGodotArray<bool>();
            if (hasBirds == null || hasBirds.Count == 0)
                hasBirds.Add(true);

            data.TryGetValue(nameof(totalScore), out v);
            totalScore = v.AsInt64();

            data.TryGetValue(nameof(bestScore), out v);
            bestScore = v.AsInt32();

            data.TryGetValue(nameof(lastScore), out v);
            lastScore = v.AsInt32();
        }
        catch
        {
            coin = 0;
            curBirdIdx = 0;
            hasBirds = [ true ];
            totalScore = 0;
            bestScore = 0;
            lastScore = 0;
        }
    }
}
