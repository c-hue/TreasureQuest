using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PauseGameUI : MonoBehaviour
{
    [Header("Menu Panel")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] TextMeshProUGUI leftText;
    [SerializeField] TextMeshProUGUI rightText;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] Button playButton;
    [SerializeField] Button resumeButton;

    [Header("Dialogue Panel")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI dialogueText;

    [SerializeField] Sprite[] icons;
    [SerializeField] Image dialogueIcon;

    private bool gameEnded = false;
    private bool isPaused = false;
    private bool showingDialogue = false;

    // --- Input --------------------------------------------------------
    void Update()
    {
        // E to exit dialogue
        if (showingDialogue && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ResumeGame();
            return;
        }

        // Pause toggle
        if (!gameEnded && !showingDialogue && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;
            if (isPaused)
                ShowPause();
            else 
                ResumeGame();
        }
    }

    // --- Panel Logic ---------------------------------------------

    public void ShowLose()
    {
       if (gameEnded) return;
       gameEnded = true;
       isPaused = false;
       showingDialogue = false;

       menuPanel.SetActive(true); 
       leftText.text = "Game";
       rightText.text = "Over";
        Time.timeScale = 0f;
        string score = GameSession.Instance?.GetScore().ToString();
        scoreText.text = "Score: "+ score;

        resumeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        AudioManager.Instance?.PlayOneShot("loseMusic");
    }

    public void ShowWin()
    {
       if (gameEnded) return;
       gameEnded = true;
       isPaused = false;
       showingDialogue = false;

       menuPanel.SetActive(true); 
       leftText.text = "You";
       rightText.text = "Win!";
       string score = GameSession.Instance?.GetScore().ToString();
        scoreText.text = "Score: "+ score;
       Time.timeScale = 0f;

        resumeButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        AudioManager.Instance?.PlayOneShot("winMusic");
    }

    public void ShowPause()
    {
        menuPanel.SetActive(true);
        leftText.text = "Game";
        rightText.text = "Pause";
        Time.timeScale = 0f;

        string score = GameSession.Instance?.GetScore().ToString();
        scoreText.text = "Score: " + score;

        resumeButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

    public void ShowDialogue(string text, int iconIndex)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;

        dialogueIcon.sprite = icons[iconIndex];

        showingDialogue = true;
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
        GameSession.Instance?.ResetSession();
        SceneManager.LoadScene("Level1");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        menuPanel.SetActive(false);
        dialoguePanel.SetActive(false);

        showingDialogue = false;
        isPaused = false;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
