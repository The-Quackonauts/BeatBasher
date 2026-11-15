using System;
using UnityEngine;

[Serializable]
public sealed class BangEvent : ObstacleEvent
{
    [Min(1)] public int requiredBangs = 2;
    [Min(0.1f)] public float allowedSeconds = 1.5f;
}
