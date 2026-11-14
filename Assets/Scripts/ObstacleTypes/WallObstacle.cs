using UnityEngine;

public class WallObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider wallCollider;
    [SerializeField] private float cleanupAfter = -3;

    private bool wallPassed = false;

    public bool ShouldBeCleanedUp()
    {
        return transform.position.z < cleanupAfter;
    }

    public bool ShouldMoveTowardsPlayer()
    {
        return true;
    }

    public void Tick(float deltaTime)
    {
        if (wallPassed) return;

        // hit wall?
        if (transform.position.z <= 0)
        {
            HeadBangDetector player = FindFirstObjectByType<HeadBangDetector>();
            if (wallCollider.bounds.Contains(player.transform.position))
            {
                LifeManager lifeManager = FindFirstObjectByType<LifeManager>();
                lifeManager.Lives = lifeManager.Lives - 1;
            }

            wallPassed = true;
        }
    }
}
