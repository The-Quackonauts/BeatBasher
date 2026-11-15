using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    GameObject player;
    private bool sceneChanging = false;
    private int startingHeadBangCount = 0;

    private void Awake()
    {
        sceneChanging = false;
        player = GameObject.FindGameObjectWithTag("MainCamera");
        startingHeadBangCount = player.GetComponent<HeadBangDetector>().headbangCount;
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
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    
    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
