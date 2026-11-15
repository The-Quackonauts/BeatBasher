using System;
using UnityEngine;

[Serializable]
public abstract class ObstacleEvent
{
    [Min(0f)] public float arrivalTimeSeconds;
    public float x;
    public float y = 1.5f;
}
