using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScript : MonoBehaviour
{

    GameObject player;
    private bool gameStarting = false;
    private int startingHeadBangCount = 0;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("MainCamera");
        startingHeadBangCount = player.GetComponent<PlayerControls>().headbangCount;
    }

    private void Update()
    {
        if ((player.GetComponent<PlayerControls>().headbangCount > startingHeadBangCount) && (gameStarting == false))
        {
            gameStarting = true;
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
