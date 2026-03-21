using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] GameObject popupPanel;

    private bool gameEnded = false;

    // --- Panel Logic ---------------------------------------------

    public void ShowLose()
    {
       if (gameEnded) return;
       gameEnded = true;

       popupPanel.SetActive(true); 
       leftText.text = "Game";
       rightText.text = "Over";
       PauseGame();
    }

    public void ShowWin()
    {
       if (gameEnded) return;
       gameEnded = true;

       popupPanel.SetActive(true); 
       leftText.text = "You";
       rightText.text = "Win!";
       PauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    // --- Menu Options ---------------------------------------------
    public void HomeScreen()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
