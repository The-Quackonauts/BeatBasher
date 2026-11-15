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

    public bool ShouldMoveTowardsPlayer()
    {
        float dist = transform.position.z;

        if (dist < stopForHeadBangingAt && !headBangingFailed)
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
                wallToHideOnceCompleted.SetActive(false);

                return true;
            }
        }

        return true;
    }

    public void Tick(float deltaTime)
    {
        if (!headBangOngoing) return;

        // check if timed out
        if (Time.time  - headbangingStartedTimestamp > maximumTimeAllowed)
        {
            headBangingFailed = true;

            LifeManager lifeManager = FindFirstObjectByType<LifeManager>();
            lifeManager.Lives--;
            headBangOngoing = false;
        }

        // subtract headbangs if any happened in the collider
        bool playerIsInTheCollider = bashingCollider.bounds.Contains(GameObject.FindWithTag("MainCamera").transform.position);
        int currentHeadBangTotal = GameObject.FindWithTag("MainCamera").GetComponent<HeadBangDetector>().headbangCount;
        int newHeadBangs = currentHeadBangTotal - previousHeadBangCount;
        if (newHeadBangs > 0 && playerIsInTheCollider)
        {
            headBangLeftToDo -= newHeadBangs;
            bangText.text = "BANG!\n" + headBangLeftToDo;
        }
        previousHeadBangCount = currentHeadBangTotal;
    }
}
