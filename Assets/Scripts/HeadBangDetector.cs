using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class HeadBangDetector : MonoBehaviour
{
    public AudioSource headbangSoundEffect;

    public float angleThreshold = 25f;
    public float timeWindow = 0.15f;

    private readonly Queue<Vector2> history = new();

    private float lastPitch;
    private float lastTrigger = -999f;
    private int quickHeadbangs;
    private bool armed = true;

    public int headbangCount = 0;

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

        // Backward movement rearms detection (non-tiny)
        if (delta < -2f)
        {
            armed = true;
            history.Clear();
        }

        // Alternatively, rearm after 1 second
        if (now - lastTrigger >= 1f)
        {
            armed = true;
            quickHeadbangs = 0;
            headbangSoundEffect.pitch = 1f;
        }

        history.Enqueue(new Vector2(now, pitch));

        // Delete old history entries
        while (history.Count > 1 && now - history.Peek().x > timeWindow)
            history.Dequeue();

        // Check forward rotation within the time window
        if (armed && delta > 0f &&
            Mathf.DeltaAngle(history.Peek().y, pitch) >= angleThreshold)
        {
            armed = false;
            headbangSoundEffect.pitch = 1f + 0.08f * quickHeadbangs;
            quickHeadbangs = Mathf.Min(quickHeadbangs + 1, 6);
            lastTrigger = now;
            headbangCount++;
            Debug.Log("BANG!");
            headbangSoundEffect.Stop();
            headbangSoundEffect.Play();
        }
    }
}
