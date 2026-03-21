using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;

    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;

    [SerializeField] Button playButton;
    [SerializeField] Button resumeButton;

    private bool gameEnded = false;

    // --- Panel Logic ---------------------------------------------

    public void ShowLose()
    {
       if (gameEnded) return;
       gameEnded = true;

       popupPanel.SetActive(true); 
       leftText.text = "Game";
       rightText.text = "Over";
        Time.timeScale = 0f;

        resumeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void ShowWin()
    {
       if (gameEnded) return;
       gameEnded = true;

       popupPanel.SetActive(true); 
       leftText.text = "You";
       rightText.text = "Win!";
       Time.timeScale = 0f;

        resumeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
    }

    public void ShowPause()
    {
        popupPanel.SetActive(true);
        leftText.text = "Game";
        rightText.text = "Pause";
        Time.timeScale = 0f;

        resumeButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
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

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        popupPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
