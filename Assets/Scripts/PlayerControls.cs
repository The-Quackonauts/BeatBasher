using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public AudioSource music;

    public float angleThreshold = 25f;
    public float timeWindow = 0.15f;

    private readonly Queue<Vector2> history = new();

    private float lastPitch;
    private float lastTrigger = -999f;
    private bool armed = true;

    void Start()
    {
        lastPitch = transform.localEulerAngles.x;
    }

    void Update()
    {
        float now = Time.unscaledTime;
        float pitch = transform.localEulerAngles.x;
        float delta = Mathf.DeltaAngle(lastPitch, pitch);
        lastPitch = pitch;

        // Backward movement immediately rearms detection
        if (delta < 0f)
        {
            armed = true;
            history.Clear();
        }

        // Alternatively, rearm after 1 second
        if (now - lastTrigger >= 1f)
            armed = true;

        history.Enqueue(new Vector2(now, pitch));

        // Delete old history entries
        while (history.Count > 1 && now - history.Peek().x > timeWindow)
            history.Dequeue();

        // Check forward rotation within the time window
        if (armed && delta > 0f &&
            Mathf.DeltaAngle(history.Peek().y, pitch) >= angleThreshold)
        {
            armed = false;
            lastTrigger = now;
            Restart();
        }
    }

    void Restart()
    {
        music.Stop();
        music.Play();
    }
}