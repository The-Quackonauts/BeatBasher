using TMPro;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource music;
    [SerializeField] private TextMeshPro timeInSongText;
    [SerializeField] private TextMeshPro countdownText;
    [SerializeField] private GameObject heartsDisplay;
    [SerializeField] private Material roomLineMaterial;

    [SerializeField] public float musicOffsetSeconds = 10f;
    private float musicCurrentTimeSeconds = 0f;
    private int displayedSecond = -1;
    private int displayedCountdown = -1;
    private double songStartDspTime;
    private bool songStarted;
    private MeshRenderer[] roomLines;
    private Material countdownMaterial;

    GameObject sceneChanger;


    void Start()
    {
        roomLines = System.Array.FindAll(GameObject.Find("Room").GetComponentsInChildren<MeshRenderer>(true),
            renderer => renderer.sharedMaterial == roomLineMaterial);
        foreach (var line in roomLines) line.enabled = false;
        heartsDisplay.SetActive(false);
        timeInSongText.gameObject.SetActive(false);

        countdownText.gameObject.SetActive(true);
        countdownMaterial = new Material(countdownText.fontSharedMaterial);
        countdownText.fontMaterial = countdownMaterial;
        countdownText.color = Color.white;

        songStartDspTime = AudioSettings.dspTime + musicOffsetSeconds;
        musicCurrentTimeSeconds = -musicOffsetSeconds;
        music.PlayScheduled(songStartDspTime);
        ShowCountdown(Mathf.CeilToInt(musicOffsetSeconds));
    }

    void Update()
    {
        if (!songStarted)
        {
            if (AudioSettings.dspTime >= songStartDspTime && music.isPlaying)
            {
                songStarted = true;
                countdownText.gameObject.SetActive(false);
                timeInSongText.gameObject.SetActive(true);
                heartsDisplay.SetActive(true);
                foreach (var line in roomLines) line.enabled = true;
            }
            else
            {
                musicCurrentTimeSeconds = (float)(AudioSettings.dspTime - songStartDspTime);
                ShowCountdown(Mathf.Max(1, Mathf.CeilToInt(-musicCurrentTimeSeconds)));
                return;
            }
        }

        musicCurrentTimeSeconds = music.time;

        int second = Mathf.Max(0, Mathf.FloorToInt(musicCurrentTimeSeconds));
        if (second != displayedSecond)
        {
            timeInSongText.text = $"{second / 60:00}:{second % 60:00}";
            displayedSecond = second;
        }

        endOfSong();
    }

    private void ShowCountdown(int number)
    {
        if (displayedCountdown == number) return;

        displayedCountdown = number;
        countdownText.text = number.ToString();
        float intensity = (10f - number) / 9f;
        countdownMaterial.SetColor("_FaceColor", Color.Lerp(Color.white, new Color(4f, 0f, 0f), intensity));
    }

    public float getMusicCurrentTime()
    {
        return musicCurrentTimeSeconds;
    }

    private void OnDestroy()
    {
        if (countdownMaterial != null) Destroy(countdownMaterial);
    }

    private void endOfSong()
    {
        if (songStarted && !music.isPlaying)
        {
            sceneChanger = GameObject.FindGameObjectWithTag("SceneManager");
            sceneChanger.GetComponent<SceneChanger>().WinGame();
        }
    }

}
