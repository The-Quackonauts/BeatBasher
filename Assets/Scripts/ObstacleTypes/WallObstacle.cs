using UnityEngine;

public class WallObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider wallCollider;

    private bool wallPassed = false;

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
                lifeManager.Lives--;
            }

            wallPassed = true;
        }
    }
}
