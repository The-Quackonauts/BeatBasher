using UnityEngine;

public class NeonStrip : MonoBehaviour
{

    private Renderer stripRenderer;
    private GameObject colourManager;

    private void Awake()
    {
        stripRenderer = GetComponentInChildren<Renderer>();
        colourManager = GameObject.FindGameObjectWithTag("ColourManager");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stripRenderer = GetComponentInChildren<Renderer>();
        colourManager = GameObject.FindGameObjectWithTag("ColourManager");
    }

    // Update is called once per frame
    void Update()
    {
        stripRenderer.material.SetColor("_EmissionColor", colourManager.GetComponent<ColourManager>().getCurrentColour() * 100);
    }
}
