using UnityEngine;

public class RingObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider ringPassthroughCollider;

    private GameObject colourManager;
    private Renderer ringRenderer;
    private bool ringPassed = false;

    private void Awake()
    {
        ringRenderer = GetComponentInChildren<Renderer>();
        colourManager = GameObject.FindGameObjectWithTag("ColourManager");
    }
    public void Configure(ObstacleEvent obstacleEvent) 
    {
        ringRenderer.material.SetColor("_EmissionColor", colourManager.GetComponent<ColourManager>().getCurrentColour() * 100);
    }

    public bool ShouldMoveTowardsPlayer()
    {
        return true;
    }

    public void Tick(float deltaTime)
    {
        if (ringPassed) return;

        Vector3 head = FindFirstObjectByType<HeadBangDetector>().transform.position;
        Bounds opening = ringPassthroughCollider.bounds;
        if (opening.Contains(head))
        {
            FindFirstObjectByType<ScoreManager>().RecordSuccess(10);
            ringPassed = true;
        }
        else if (opening.max.z < head.z)
        {
            FindFirstObjectByType<LifeManager>().Lives--;
            ringPassed = true;
        }
    }
}
