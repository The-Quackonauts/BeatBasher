using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private GameObject ringPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject bangPrefab;
    [SerializeField] private float spawnDistance = 20f;
    [SerializeField] private Vector3 obstacleMoveVectorPerSecond = new(0f, 0f, -2f);
    [SerializeReference] private List<ObstacleEvent> schedule = new();

    private List<ObstacleEvent> orderedSchedule;
    private int nextEventIndex;
    private Transform player;

    private void Start()
    {
        player = FindFirstObjectByType<HeadBangDetector>().transform;
        orderedSchedule = new List<ObstacleEvent>(schedule);
        orderedSchedule.Sort((a, b) => a.arrivalTimeSeconds.CompareTo(b.arrivalTimeSeconds));
    }

    private void Update()
    {
        float songTime = musicManager.getMusicCurrentTime();
        // move, tick & clean-up children
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            IObstacle obstacle = child.GetComponent<IObstacle>();
            if (obstacle.ShouldMoveTowardsPlayer())
                child.position += obstacleMoveVectorPerSecond * Time.deltaTime;

            obstacle.Tick(Time.deltaTime);
            if (child.position.z <= -3f)
            {
                Destroy(child.gameObject);
            }
        }

        // spawn stuff that is upcoming
        float travelTime = spawnDistance / -obstacleMoveVectorPerSecond.z;
        while (nextEventIndex < orderedSchedule.Count &&
               orderedSchedule[nextEventIndex].arrivalTimeSeconds <= songTime + travelTime)
        {
            Spawn(orderedSchedule[nextEventIndex], songTime);
            nextEventIndex++;
        }
    }

    private void Spawn(ObstacleEvent entry, float songTime)
    {
        GameObject prefab = entry switch
        {
            RingEvent => ringPrefab,
            WallEvent => wallPrefab,
            BangEvent => bangPrefab,
            _ => null
        };

        float z = player.position.z - obstacleMoveVectorPerSecond.z * (entry.arrivalTimeSeconds - songTime);
        GameObject instance = Instantiate(prefab, new Vector3(entry.x, entry.y, z),
            Quaternion.identity, transform);
        IObstacle obstacle = instance.GetComponent<IObstacle>();
        obstacle.Configure(entry);
    }
}
