using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ColourManager : MonoBehaviour
{
    public Material colourMaterial;
    Color currentColour = Color.blue;

    private void Awake()
    {
        setColour(currentColour);
    }

    public Color getCurrentColour()
    {
        return currentColour;
    }

    public void setColour(Color colour)
    {
        currentColour = colour;
        RenderSettings.fogColor = currentColour * 0.5f;
    }
}
