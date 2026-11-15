using UnityEngine;

public class RingObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider ringPassthroughCollider;

    private ColourManager colourManager;
    private Renderer ringRenderer;
    private RingEvent ringEvent;
    private bool ringPassed = false;

    private void Awake()
    {
        ringRenderer = GetComponentInChildren<Renderer>();
        colourManager = FindFirstObjectByType<ColourManager>();
    }
    public void Configure(ObstacleEvent obstacleEvent) 
    {
        ringEvent = (RingEvent)obstacleEvent;
        UpdateColour();
    }

    public bool ShouldMoveTowardsPlayer()
    {
        return true;
    }

    public void Tick(float deltaTime)
    {
        UpdateColour();
        if (ringPassed) return;

        Vector3 head = FindFirstObjectByType<HeadBangDetector>().transform.position;
        Bounds opening = ringPassthroughCollider.bounds;
        if (opening.Contains(head))
        {
            FindFirstObjectByType<ScoreManager>().RecordSuccess(10);
            ringPassed = true;
            if (ringEvent.colourMode == RingColourMode.Rainbow)
                colourManager.StartRainbow(ColourManager.RainbowColour(Time.time / 15f));
            else if (ringEvent.colourMode == RingColourMode.Solid)
                colourManager.setColour(ringEvent.colour);
        }
        else if (opening.max.z < head.z)
        {
            FindFirstObjectByType<LifeManager>().Lives--;
            ringPassed = true;
        }
    }

    private void UpdateColour()
    {
        Color colour = ringEvent.colourMode switch
        {
            RingColourMode.Solid => ColourManager.MatchOriginalBrightness(ringEvent.colour),
            RingColourMode.Rainbow => ringPassed ? colourManager.getCurrentColour()
                : ColourManager.RainbowColour(Time.time / 15f),
            _ => colourManager.getCurrentColour()
        };
        ringRenderer.material.SetColor("_EmissionColor", colour * 100);
    }
}
