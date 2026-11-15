using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private TextMeshPro finalScoreText;
    GameObject player;
    private bool sceneChanging = false;
    private int startingHeadBangCount = 0;

    private void Awake()
    {
        sceneChanging = false;
        player = GameObject.FindGameObjectWithTag("MainCamera");
        startingHeadBangCount = player.GetComponent<HeadBangDetector>().headbangCount;
    }

    private void Start()
    {
        if (finalScoreText != null)
            finalScoreText.text = $"Score: {ScoreManager.LastScore}";
    }

    private void Update()
    {
        if ((player.GetComponent<HeadBangDetector>().headbangCount > startingHeadBangCount) && (sceneChanging == false))
        {
            sceneChanging = true;
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            if (currentScene == 0) // main menu
            {
                StartGame();
            }
            else if (currentScene == 2 || currentScene == 3) // lose and win screen
            {
                ResetGame();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void LoseGame()
    {
        SceneManager.LoadScene(3);
    }

    public void WinGame()
    {
        SceneManager.LoadScene(2);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
