using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Material ringMaterial;
    [SerializeField, Min(0.01f)] private float ringRadius = 0.55f;
    [SerializeField] private TextMeshPro scoreLabel;
    [SerializeField] private TextMeshPro maxLabel;

    private const int MaxMultiplier = 8;
    private const int RingSegments = 128;
    private const float FillSeconds = 0.5f;
    private const float FadeSeconds = 0.25f;
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    private struct FillStep
    {
        public float Target;
        public int NextMultiplier;

        public FillStep(float target, int nextMultiplier)
        {
            Target = target;
            NextMultiplier = nextMultiplier;
        }
    }

    private enum FillPhase { Idle, Filling, Fading }

    private readonly Queue<FillStep> pendingFills = new();
    private ColourManager colourManager;
    private Color sceneColour;
    private TextMeshPro multiplierText;
    private LineRenderer ringTrack;
    private LineRenderer ringProgress;
    private MaterialPropertyBlock trackProperties;
    private MaterialPropertyBlock progressProperties;
    private bool visible = true;
    private int progress;
    private int displayedMultiplier = 1;
    private int completedMultiplier;
    private float displayedFill;
    private float fillStart;
    private float fillTarget;
    private float fillDuration = FillSeconds;
    private float phaseElapsed;
    private float fadeStrength = 1f;
    private float renderedRadius;
    private FillPhase phase;

    public int Score { get; private set; }
    public int Multiplier { get; private set; } = 1;
    public static int LastScore { get; private set; }

    private void Awake()
    {
        LastScore = 0;
        colourManager = FindFirstObjectByType<ColourManager>();
        multiplierText = GetComponent<TextMeshPro>();
        ringTrack = CreateRing("Multiplier Track", 0.026f);
        ringProgress = CreateRing("Multiplier Progress", 0.042f);
        trackProperties = new MaterialPropertyBlock();
        progressProperties = new MaterialPropertyBlock();

        ringTrack.loop = true;
        DrawTrack();

        RefreshText();
        ApplyColour(colourManager.getCurrentColour());
        RenderRing();
    }

    private void Update()
    {
        if (ringRadius != renderedRadius)
        {
            DrawTrack();
            RenderRing();
        }
        Color currentColour = colourManager.getCurrentColour();
        if (currentColour != sceneColour) ApplyColour(currentColour);
        if (phase == FillPhase.Idle) return;

        phaseElapsed += Time.deltaTime;
        if (phase == FillPhase.Filling)
        {
            float t = Mathf.Clamp01(phaseElapsed / fillDuration);
            displayedFill = Mathf.Lerp(fillStart, fillTarget, Mathf.SmoothStep(0f, 1f, t));
            if (t >= 1f)
            {
                displayedFill = fillTarget;
                if (completedMultiplier > 0)
                {
                    phase = FillPhase.Fading;
                    phaseElapsed = 0f;
                }
                else BeginNextFill();
            }
        }
        else
        {
            fadeStrength = 1f - Mathf.Clamp01(phaseElapsed / FadeSeconds);
            if (fadeStrength <= 0f)
            {
                displayedMultiplier = completedMultiplier;
                displayedFill = 0f;
                fadeStrength = 1f;
                RefreshText();
                BeginNextFill();
            }
        }
        RenderRing();
    }

    public void RecordSuccess(int basePoints)
    {
        if (Multiplier == 0) Multiplier = 1;

        Score += basePoints * Multiplier;
        LastScore = Score;
        if (Multiplier < MaxMultiplier)
        {
            int required = Mathf.Min(1 << Multiplier, 10);
            progress++;
            int nextMultiplier = 0;
            float target = (float)progress / required;
            if (progress >= required)
            {
                Multiplier++;
                progress = 0;
                nextMultiplier = Multiplier;
            }
            pendingFills.Enqueue(new FillStep(target, nextMultiplier));
            if (phase == FillPhase.Idle) BeginNextFill();
        }
        if (displayedMultiplier == 0) displayedMultiplier = 1;
        RefreshText();
    }

    public void ResetStreak()
    {
        Multiplier = 0;
        progress = 0;
        displayedMultiplier = 0;
        displayedFill = 0f;
        fadeStrength = 1f;
        phase = FillPhase.Idle;
        pendingFills.Clear();
        RefreshText();
        RenderRing();
    }

    public void SetVisible(bool isVisible)
    {
        visible = isVisible;
        multiplierText.renderer.enabled = visible;
        scoreLabel.renderer.enabled = visible;
        maxLabel.renderer.enabled = visible;
        ringTrack.enabled = visible;
        RenderRing();
    }

    private LineRenderer CreateRing(string ringName, float width)
    {
        var ringObject = new GameObject(ringName);
        ringObject.transform.SetParent(transform, false);
        Vector3 scale = transform.localScale;
        ringObject.transform.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);

        var ring = ringObject.AddComponent<LineRenderer>();
        ring.sharedMaterial = ringMaterial;
        ring.useWorldSpace = false;
        ring.alignment = LineAlignment.TransformZ;
        ring.widthMultiplier = width;
        ring.numCornerVertices = 4;
        ring.numCapVertices = 4;

        return ring;
    }

    private void DrawTrack()
    {
        ringTrack.positionCount = RingSegments;
        for (int i = 0; i < RingSegments; i++)
            ringTrack.SetPosition(i, RingPoint(i, 0.01f));
        renderedRadius = ringRadius;
    }

    private Vector3 RingPoint(float segment, float depth)
    {
        float angle = segment * Mathf.PI * 2f / RingSegments;
        return new Vector3(Mathf.Sin(angle) * ringRadius, Mathf.Cos(angle) * ringRadius, depth);
    }

    private void RefreshText()
    {
        scoreLabel.text = $"<size=24>SCORE</size>\n{Score:N0}";
        multiplierText.text = $"{displayedMultiplier}x";
        maxLabel.text = displayedMultiplier == MaxMultiplier ? "MAX" : "";
        multiplierText.ForceMeshUpdate();
        Vector3 center = multiplierText.textBounds.center;
        ringTrack.transform.localPosition = center;
        ringProgress.transform.localPosition = center;
    }

    private void BeginNextFill()
    {
        if (pendingFills.Count == 0)
        {
            phase = FillPhase.Idle;
            return;
        }

        fillDuration = Mathf.Max(0.15f, FillSeconds / pendingFills.Count);
        FillStep step = pendingFills.Dequeue();
        fillStart = displayedFill;
        fillTarget = step.Target;
        completedMultiplier = step.NextMultiplier;
        phaseElapsed = 0f;
        phase = FillPhase.Filling;
    }

    private void ApplyColour(Color colour)
    {
        sceneColour = colour;
        Color textColour = Color.Lerp(Color.white, colour, 0.7f);
        scoreLabel.color = textColour;
        multiplierText.color = textColour;
        maxLabel.color = textColour;

        trackProperties.SetColor(BaseColor, colour * 0.14f);
        trackProperties.SetColor(EmissionColor, colour * 0.3f);
        ringTrack.SetPropertyBlock(trackProperties);
        SetProgressColour();
    }

    private void SetProgressColour()
    {
        ringProgress.widthMultiplier = Mathf.Lerp(0.026f, 0.042f, fadeStrength);
        progressProperties.SetColor(BaseColor, Color.Lerp(sceneColour * 0.14f, Color.white, fadeStrength));
        progressProperties.SetColor(EmissionColor, sceneColour * Mathf.Lerp(0.3f, 100f, fadeStrength));
        ringProgress.SetPropertyBlock(progressProperties);
    }

    private void RenderRing()
    {
        ringProgress.enabled = visible && displayedFill > 0f && displayedMultiplier < MaxMultiplier;
        if (!ringProgress.enabled) return;

        float arc = displayedFill * RingSegments;
        int segments = Mathf.CeilToInt(arc);
        ringProgress.positionCount = segments + 1;
        for (int i = 0; i < segments; i++)
            ringProgress.SetPosition(i, RingPoint(i, -0.01f));
        ringProgress.SetPosition(segments, RingPoint(arc, -0.01f));
        SetProgressColour();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Application.isPlaying ? sceneColour : Color.blue;
        TextMeshPro text = GetComponent<TextMeshPro>();
        Vector3 center = transform.TransformPoint(text.textBounds.center);
        for (int i = 0; i < RingSegments; i++)
        {
            Vector3 a = center + transform.rotation * RingPoint(i, 0.01f);
            Vector3 b = center + transform.rotation * RingPoint((i + 1) % RingSegments, 0.01f);
            Gizmos.DrawLine(a, b);
        }
    }
}
