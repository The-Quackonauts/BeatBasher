using UnityEngine;

interface IObstacle
{
    bool ShouldMoveTowardsPlayer();
    bool ShouldBeCleanedUp();
    void Tick(float deltaTime);
}
