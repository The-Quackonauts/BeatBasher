using UnityEngine;

public class WallObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider wallCollider;

    private bool wallPassed = false;

    private GameObject colourManager;
    private Renderer[] wallRenderer;

    private void Awake()
    {
        wallRenderer = GetComponentsInChildren<Renderer>();
        colourManager = GameObject.FindGameObjectWithTag("ColourManager");
    }


    public void Configure(ObstacleEvent obstacleEvent) 
    {
        foreach (Renderer renderer in wallRenderer)
        {
            foreach(Material material in renderer.materials)
            { 
                renderer.material.SetColor("_EmissionColor", colourManager.GetComponent<ColourManager>().getCurrentColour() * 100);
            }
        }
    }

    public bool ShouldMoveTowardsPlayer()
    {
        return true;
    }

    public void Tick(float deltaTime)
    {
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
}
