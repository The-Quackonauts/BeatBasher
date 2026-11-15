using UnityEngine;

public class RingObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider ringPassthroughCollider;

    private bool ringPassed = false;

    public void Configure(ObstacleEvent obstacleEvent) { }

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
            ringPassed = true;
        }
        else if (opening.max.z < head.z)
        {
            FindFirstObjectByType<LifeManager>().Lives--;
            ringPassed = true;
        }
    }
}
