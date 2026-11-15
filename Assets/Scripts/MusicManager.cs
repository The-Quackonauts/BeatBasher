using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{

    public AudioSource music;

    [SerializeField] public float musicOffsetSeconds = 5f;
    private float musicCurrentTimeSeconds = 0f;

    GameObject sceneChanger;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        music.PlayDelayed(musicOffsetSeconds);

        musicCurrentTimeSeconds = 0 - musicOffsetSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if(musicCurrentTimeSeconds < 0)
        {
            musicCurrentTimeSeconds += Time.deltaTime;
        }
        else
        {
            musicCurrentTimeSeconds = music.time;
        }

        endOfSong();
    }

    public float getMusicCurrentTime()
    {
        return musicCurrentTimeSeconds;
    }

    private void endOfSong()
    {
        if (!music.isPlaying)
        {
            sceneChanger = GameObject.FindGameObjectWithTag("SceneManager");
            sceneChanger.GetComponent<SceneChanger>().WinGame();
        }
    }

}
