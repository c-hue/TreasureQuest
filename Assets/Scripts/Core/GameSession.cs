using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    // ─── Singleton ───────────────────────────────────────────────────────
    public static GameSession Instance { get; private set; }

    // ─── State ───────────────────────────────────────────────────────────
    [SerializeField] int lives = 3;
    int score = 0;
    int mapPieces;
    public bool mapFound = false;
    public bool keyFound = false;
    GameObject mapCounter;
    GameObject scoreCounter;
    
    void Start()
    {
        mapPieces = 0;
        mapCounter = GameObject.Find("MapCounter");
        scoreCounter = GameObject.Find("ScoreText");
    }
    
    void Update()
    {
        if (mapCounter != null)
        {
            TextMeshProUGUI mapText = mapCounter.GetComponent<TextMeshProUGUI>();
            mapText.text = mapPieces.ToString();
            if (mapPieces == 4)
            {
                mapFound = true;
            }
        }

        if (scoreCounter != null)
        {
            TextMeshProUGUI scoreText = scoreCounter.GetComponent<TextMeshProUGUI>();
            scoreText.text = score.ToString();
        }
    }

    void Awake()
    {
        // Singleton pattern — one GameSession persists across scenes
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ─── Map ───────────────────────────────────────────────────────────
    public void AddMap()
    {
        mapPieces += 1;
    }
    // ─── Key ───────────────────────────────────────────────────────────
    public void KeyFound()
    {
        keyFound = true;
    }

    // ─── Score ───────────────────────────────────────────────────────────
    public void AddScore(int points)
    {
        score += points;
    }

    int GetScore() => score;

    // ─── Lives ───────────────────────────────────────────────────────────
    public void ProcessPlayerDeath()
    {
        lives--;
        if (lives > 0)
            ReloadCurrentScene();
        else
            LoadGameOver();
    }

    // ─── Scene Loading ───────────────────────────────────────────────────
    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadGameOver()
    {
        FindFirstObjectByType<EndGameUI>().ShowLose();
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(nextIndex);
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            FindFirstObjectByType<EndGameUI>().ShowWin();
    }
}