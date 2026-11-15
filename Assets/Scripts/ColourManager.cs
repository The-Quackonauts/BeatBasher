using UnityEngine;

public class ColourManager : MonoBehaviour
{
    public Material colourMaterial;
    private const float BlueLuminance = 0.0722f;
    private Color currentColour = Color.blue;
    private bool rainbow;
    private float rainbowStart;
    private float rainbowStartHue;

    private void Awake()
    {
        ApplyColour(currentColour);
    }

    private void Update()
    {
        if (rainbow)
            ApplyColour(RainbowColour(rainbowStartHue + (Time.time - rainbowStart) / 15f));
    }

    public void StartRainbow(Color colour)
    {
        if (rainbow) return;
        setColour(colour);
        Color.RGBToHSV(currentColour, out rainbowStartHue, out _, out _);
        rainbowStart = Time.time;
        rainbow = true;
    }

    public static Color RainbowColour(float hue)
    {
        // Match the old blue's light output so changing hue does not boost bloom.
        Color colour = Color.HSVToRGB(Mathf.Repeat(hue, 1f), 1f, 1f);
        float luminance = colour.r * 0.2126f + colour.g * 0.7152f + colour.b * 0.0722f;
        float scale = BlueLuminance / luminance;
        return new Color(colour.r * scale, colour.g * scale, colour.b * scale, 1f);
    }

    public static Color MatchOriginalBrightness(Color colour)
    {
        if (colour.maxColorComponent <= 0f) return Color.black;
        Color.RGBToHSV(colour, out float hue, out _, out _);
        return RainbowColour(hue);
    }

    public Color getCurrentColour()
    {
        return currentColour;
    }

    public void setColour(Color colour)
    {
        rainbow = false;
        ApplyColour(MatchOriginalBrightness(colour));
    }

    private void ApplyColour(Color colour)
    {
        currentColour = colour;
        RenderSettings.fogColor = currentColour * 0.5f;
    }
}
