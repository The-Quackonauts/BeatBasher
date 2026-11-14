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
        if (((int) Random.Range(0f, 250f)) == 42)
        {
            GameObject newObstacle = Instantiate(ringObstaclePrefab, obstacleSpawnBasePosition, Quaternion.identity);
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
