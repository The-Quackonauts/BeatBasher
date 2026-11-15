using System;
using TMPro;
using UnityEngine;

public class BangObstacle : MonoBehaviour, IObstacle
{
    [SerializeField] private BoxCollider bashingCollider;
    [SerializeField] private TextMeshPro bangText;
    [SerializeField] private GameObject wallToHideOnceCompleted;
    [SerializeField] private float stopForHeadBangingAt = 0.5f;
    [SerializeField] private float maximumTimeAllowed = 1.5f;

    private bool headBangOngoing = false;
    private int headBangLeftToDo = 3;
    private bool headBangingFailed = false;
    private int previousHeadBangCount = 0;
    private float headbangingStartedTimestamp;

    private GameObject colourManager;
    private Renderer[] bangRenderer;

    private void Awake()
    {
        bangRenderer = GetComponentsInChildren<Renderer>();
        colourManager = GameObject.FindGameObjectWithTag("ColourManager");
    }

    public void Configure(ObstacleEvent obstacleEvent)
    {
        BangEvent bangEvent = (BangEvent)obstacleEvent;
        headBangLeftToDo = bangEvent.requiredBangs;
        maximumTimeAllowed = bangEvent.allowedSeconds;
        bangText.text ="\n" + headBangLeftToDo;

        foreach (Renderer renderer in bangRenderer)
        {
            renderer.material.SetColor("_EmissionColor", colourManager.GetComponent<ColourManager>().getCurrentColour() * 100);
        }
    }

    public bool ShouldMoveTowardsPlayer()
    {
        float dist = transform.position.z - GameObject.FindWithTag("MainCamera").transform.position.z;

        if ((dist < stopForHeadBangingAt || headBangOngoing) && !headBangingFailed)
        {
            if (headBangLeftToDo > 0)
            {
                if (!headBangOngoing)
                {
                    headBangOngoing = true;
                    previousHeadBangCount = GameObject.FindWithTag("MainCamera").GetComponent<HeadBangDetector>().headbangCount;
                    headbangingStartedTimestamp = Time.time;
                }

                return false;
            }
            else
            {
                headBangOngoing = false;
                return true;
            }
        }

        return true;
    }

    public void Tick(float deltaTime)
    {
        if (!headBangOngoing) return;

        // Count nods while the head lines up with the opening.
        HeadBangDetector player = GameObject.FindWithTag("MainCamera").GetComponent<HeadBangDetector>();
        Bounds box = bashingCollider.bounds;
        Vector3 head = player.transform.position;
        bool playerIsInTheCollider = head.x >= box.min.x && head.x <= box.max.x &&
                                     head.y >= box.min.y && head.y <= box.max.y;
        int currentHeadBangTotal = player.headbangCount;
        int newHeadBangs = currentHeadBangTotal - previousHeadBangCount;
        if (newHeadBangs > 0 && playerIsInTheCollider)
        {
            headBangLeftToDo = Mathf.Max(0, headBangLeftToDo - newHeadBangs);
            bangText.text = "\n" + headBangLeftToDo;
        }
        previousHeadBangCount = currentHeadBangTotal;

        if (headBangLeftToDo <= 0)
        {
            wallToHideOnceCompleted.SetActive(false);
            headBangOngoing = false;
            return;
        }

        // check if timed out
        if (Time.time - headbangingStartedTimestamp > maximumTimeAllowed)
        {
            headBangingFailed = true;
            FindFirstObjectByType<LifeManager>().Lives--;
            headBangOngoing = false;
        }
    }
}
