using UnityEngine;

public class WallObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider wallCollider;

    private bool wallPassed = false;

    private ColourManager colourManager;
    private Renderer[] wallRenderer;

    private void Awake()
    {
        wallRenderer = GetComponentsInChildren<Renderer>();
        colourManager = FindFirstObjectByType<ColourManager>();
    }


    public void Configure(ObstacleEvent obstacleEvent) 
    {
        UpdateColour();
    }

    public bool ShouldMoveTowardsPlayer()
    {
        return true;
    }

    public void Tick(float deltaTime)
    {
        UpdateColour();
        if (wallPassed) return;

        Vector3 head = FindFirstObjectByType<HeadBangDetector>().transform.position;
        Bounds wall = wallCollider.bounds;
        if (wall.Contains(head))
        {
            FindFirstObjectByType<LifeManager>().Lives--;
            wallPassed = true;
        }
        else if (wall.max.z < head.z)
        {
            FindFirstObjectByType<ScoreManager>().RecordSuccess(10);
            wallPassed = true;
        }
    }

    private void UpdateColour()
    {
        foreach (Renderer renderer in wallRenderer)
            renderer.material.SetColor("_EmissionColor", colourManager.getCurrentColour() * 100);
    }
}
