using UnityEngine;

public class LifeManager : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] hearts;

    private const int MaxLives = 5;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    private static readonly Color SpentHeart = new Color(0.35f, 0.06f, 0.06f, 1f);

    GameObject sceneChanger;

    private int lives = MaxLives;

    private void Awake()
    {
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        var properties = new MaterialPropertyBlock();
        properties.SetColor(EmissionColor, Color.black);
        properties.SetColor(BaseColor, SpentHeart);
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].SetPropertyBlock(i < lives ? null : properties);
    }

    public int Lives
    {
        get { return lives; }
        set
        {
            lives = Mathf.Clamp(value, 0, MaxLives);
            UpdateHearts();
            if (lives <= 0)
            {
                sceneChanger = GameObject.FindGameObjectWithTag("SceneManager");
                sceneChanger.GetComponent<SceneChanger>().LoseGame();
            }
        }
    }
}
