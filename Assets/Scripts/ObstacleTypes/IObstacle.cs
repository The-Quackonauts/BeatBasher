using UnityEngine;

interface IObstacle
{
    bool ShouldMoveTowardsPlayer();
    void Tick(float deltaTime);
}
