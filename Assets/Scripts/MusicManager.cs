using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource music;

    [SerializeField] public float musicOffset = 5000f;
    private float musicCurrentTime = 0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        music.Play();
        music.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        musicCurrentTime += Time.deltaTime;
        UnityEngine.Debug.Log(musicCurrentTime);
    }
}
