using System;
using UnityEngine;

public enum RingColourMode { Keep, Solid, Rainbow }

[Serializable]
public sealed class RingEvent : ObstacleEvent
{
    public Color colour = Color.blue;
    public RingColourMode colourMode;
}
