using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] hearts;

    private const int MaxLives = 5;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    GameObject sceneChanger;

    private int lives = MaxLives;
    private ColourManager colourManager;
    private Color displayedColour;

    private void Awake()
    {
        colourManager = FindFirstObjectByType<ColourManager>();
        UpdateHearts();
    }

    private void Update()
    {
        if (colourManager.getCurrentColour() != displayedColour) UpdateHearts();
    }

    private void UpdateHearts()
    {
        displayedColour = colourManager.getCurrentColour();
        var lit = new MaterialPropertyBlock();
        lit.SetColor(EmissionColor, displayedColour * 100f);
        lit.SetColor(BaseColor, Color.white);
        var spent = new MaterialPropertyBlock();
        spent.SetColor(EmissionColor, Color.black);
        spent.SetColor(BaseColor, new Color(displayedColour.r * 0.3f,
            displayedColour.g * 0.3f, displayedColour.b * 0.3f, 1f));
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetPropertyBlock(i < lives ? lit : spent);
    }

    public int Lives
    {
        get { return lives; }
        set
        {
            int nextLives = Mathf.Clamp(value, 0, MaxLives);
            if (nextLives < lives) FindFirstObjectByType<ScoreManager>().ResetStreak();
            lives = nextLives;
            UpdateHearts();
            if (lives <= 0)
            {
                sceneChanger = GameObject.FindGameObjectWithTag("SceneManager");
                sceneChanger.GetComponent<SceneChanger>().LoseGame();
            }
        }
    }
}
