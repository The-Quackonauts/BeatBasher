using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject ringObstaclePrefab;
    [SerializeField] private Vector3 obstacleSpawnBasePosition;
    [SerializeField] private Vector3 obstacleMoveVectorPerSecond;


    void Start()
    {
        
    }

    void Update()
    {
        // spawn new rings
        if (((int) Random.Range(0f, 500f)) == 42)
        {
            GameObject newObstacle = Instantiate(ringObstaclePrefab, obstacleSpawnBasePosition, Quaternion.identity);
            newObstacle.transform.parent = transform;
        }

        // move children towards player
        for (int i = 0, count = transform.childCount; i < count; i++)
        {
            Transform child = transform.GetChild(i);
            child.position += obstacleMoveVectorPerSecond * Time.fixedDeltaTime;
        }
    }
}
