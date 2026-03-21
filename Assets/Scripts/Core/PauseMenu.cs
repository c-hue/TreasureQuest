using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
            FindFirstObjectByType<EndGameUI>().ShowPause();
        else 
            FindFirstObjectByType<EndGameUI>().ResumeGame();
    }
}