using Godot;
using Godot.Collections;
using System;

namespace FlappyBird;

public partial class BirdTemplateGroup : Resource
{
    [Export]
    Array<BirdTemplate> templates;

    static BirdTemplateGroup instance;

    public int Count => templates.Count;

    public BirdTemplate this[int index] => templates[index];

    public static BirdTemplateGroup Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GD.Load<BirdTemplateGroup>(
                    "res://flappy-bird/resources/BirdTemplateGroup.tres"
                );
            }
            return instance;
        }
    }

    public int IndexOf(BirdTemplate template) => templates.IndexOf(template);
}
