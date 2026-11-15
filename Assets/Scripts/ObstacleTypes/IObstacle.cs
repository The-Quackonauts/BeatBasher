using UnityEngine;

interface IObstacle
{
    void Configure(ObstacleEvent obstacleEvent);
    bool ShouldMoveTowardsPlayer();
    void Tick(float deltaTime);
}
