using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;

namespace FlappyBird;

public partial class PipesManager : Node2D
{
    [Export]
    PackedScene pipesPrefab;
    [Export]
    float spawnDelay = 1f;
    [Export]
    float spawnPosX = 200f;
    [Export]
    float minSpawnPosY = -100f;
    [Export]
    float maxSpawnPosY = 100f;
    [Export]
    float despawnPosX = -100f;

    Array<Pipes> pipesList = new();
    Queue<Pipes> pipesPool = new();
    float spawnTimer = 0;

    public bool IsEnable
    {
        get;
        set;
    }

    public float MoveSpeed
    {
        get;
        set;
    }

    public override void _Ready()
    {
        // 랜덤함수에 시드값은 넣는다.
        GD.Randomize();
    }

    public override void _Process(double delta)
    {
        if (!IsEnable)
            return;
        UpdateSpawn((float)delta);
        UpdateMoving((float)delta);
    }

    void UpdateSpawn(float delta)
    {
        spawnTimer -= delta;
        if (spawnTimer > 0f)
            return;

        Spawn();
        spawnTimer = spawnDelay;
    }

    void UpdateMoving(float delta)
    {
        Queue<Pipes> despawns = new();
        foreach (var pipes in pipesList)
        {
            var p = pipes.GlobalPosition;
            if (p.X <= despawnPosX)
            {
                despawns.Enqueue(pipes);
                continue;
            }
            p.X -= MoveSpeed * delta;
            pipes.GlobalPosition = p;
        }
        while (despawns.Count > 0)
            Despawn(despawns.Dequeue());
    }

    void Spawn()
    {
        Pipes pipes = null;
        if (pipesPool.Count > 0)
        {
            pipes = pipesPool.Dequeue();
            pipes.Visible = true;
        }
        else
        {
            pipes = pipesPrefab.Instantiate<Pipes>();
            AddChild(pipes);
        }
        pipes.GlobalPosition = new(
            spawnPosX,
            (float)GD.RandRange(minSpawnPosY, maxSpawnPosY)
        );
        pipesList.Add(pipes);
    }

    void Despawn(Pipes pipes)
    {
        pipes.Visible = false;
        pipesList.Remove(pipes);
        pipesPool.Enqueue(pipes);
    }
}
