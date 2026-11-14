using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Vector3 obstacleSpawnBasePosition;
    [SerializeField] private Vector3 obstacleMoveVectorPerSecond;
    [SerializeField] private float xAxisRandomOffset = 0.42f;
    [SerializeField] private float yAxisDownwardRandomOffset = 0.7f;

    void Update()
    {
        // spawn new obstacles
        if (((int) Random.Range(0f, 250f)) == 42)
        {
            int randomIndex = Random.Range(0, obstaclePrefabs.Length);
            GameObject selectedPrefab = obstaclePrefabs[randomIndex];

            // apply random object offset
            float xOffset = Random.Range(-xAxisRandomOffset, xAxisRandomOffset);
            float yOffset = Random.Range(-yAxisDownwardRandomOffset, 0);
            Vector3 obstaclePos = obstacleSpawnBasePosition;
            obstaclePos.x += xOffset;
            obstaclePos.y += yOffset;

            GameObject newObstacle = Instantiate(selectedPrefab, obstaclePos, Quaternion.identity);
            newObstacle.transform.parent = transform;
        }

        // move, tick & clean-up children
        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            IObstacle obstacleScript = child.gameObject.GetComponent<IObstacle>();

            if (obstacleScript.ShouldMoveTowardsPlayer())
                child.position += obstacleMoveVectorPerSecond * Time.fixedDeltaTime;

            obstacleScript.Tick(Time.fixedDeltaTime);

            if (obstacleScript.ShouldBeCleanedUp())
            {
                Destroy(child.gameObject);
            }
        }
    }
}
