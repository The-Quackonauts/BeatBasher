using UnityEngine;

public class RingObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider ringPassthroughCollider;

    private bool ringPassed = false;

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
                lifeManager.Lives--;
            }

            ringPassed = true;
        }
    }
}
