using UnityEngine;

public class RingObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider ringPassthroughCollider;
    [SerializeField] private float cleanupAfter = -3;

    private bool ringPassed = false;

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
        if (ringPassed) return;

        // passed through / missed ring?
        if (transform.position.z <= 0)
        {
            HeadBangDetector player = FindFirstObjectByType<HeadBangDetector>();

            if (!ringPassthroughCollider.bounds.Contains(player.transform.position))
            {
                LifeManager lifeManager = FindFirstObjectByType<LifeManager>();
                lifeManager.Lives = lifeManager.Lives - 1;
            }

            ringPassed = true;
        }
    }
}
