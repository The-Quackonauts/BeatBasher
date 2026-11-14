using TMPro;
using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro livesDisplayText;

    private int lives = 5;
    public int Lives
    {
        get {  return lives; }
        set 
        {
            lives = value;
            livesDisplayText.text = "Lives: " + lives;
            // TODO: death check & restart
        }
    }
}
