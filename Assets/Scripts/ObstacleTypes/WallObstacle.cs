using UnityEngine;

public class WallObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider wallCollider;

    private bool wallPassed = false;

    public void Configure(ObstacleEvent obstacleEvent) { }

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
            wallPassed = true;
        }
    }
}
