using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro livesDisplayText;

    GameObject sceneChanger;

    private int lives = 5;
    public int Lives
    {
        get { return lives; }
        set
        {
            lives = value;
            if (lives <= 0)
            {
                sceneChanger = GameObject.FindGameObjectWithTag("SceneManager");
                sceneChanger.GetComponent<SceneChanger>().LoseGame();
            }
            livesDisplayText.text = "Lives left: " + lives;
        }
    }
}
