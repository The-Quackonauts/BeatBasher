using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{

    public InputActionProperty resetButton;
    public AudioSource music;

    // Update is called once per frame
    void Update()
    {
        if(resetButton.action.WasPressedThisFrame())
        {
            UnityEngine.Debug.Log("A");
            music.Play();
        }
    }
}
