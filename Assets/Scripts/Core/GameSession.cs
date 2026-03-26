using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class GameSession : MonoBehaviour
{
    // ─── Singleton ───────────────────────────────────────────────────────
    public static GameSession Instance { get; private set; }

    // ─── State ───────────────────────────────────────────────────────────
    [SerializeField] int lives = 3;
    int score;
    int mapPieces;
    public bool mapFound = false;
    public bool keyFound = false;
    GameObject mapCounter;
    GameObject scoreCounter;
    GameObject keyCounter;
    GameObject deathBar;
    List<string> collectedItems = new List<string>();

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        FindUIReferences();
        UpdateUI();
    }
    void Update()
    {
        //Debug.Log(lives);
        UpdateUI();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mapPieces = 0;
        FindUIReferences();
        UpdateUI();

        PauseGameUI ui = FindFirstObjectByType<PauseGameUI>();
        if (scene.name == "Level1")
        {
            ui.ShowDialogue(
                "Argh! Me crew's gone missin'! This  island be the last place I laid eyes on 'em. Mayhap I can find the map they were followin' fer some clues...",
                2
            );
        }
        else if (scene.name == "Level2")
        {
            ui.ShowDialogue(
                "Shiver me timbers! Our ship's helm be in pieces! The rest o' the ship must be nearby... an' I pray it's in better shape than this wrecked helm.",
                1
            );
        }
        else if (scene.name == "Level3")
        {
            ui.ShowDialogue(
                "This can't be!  Me own crew... cursed an' risen as the undead! I'll not let ye suffer like this. By me blade, it's time to walk the plank!",
                0
            );
        }

    }
    void FindUIReferences()
    {
        mapCounter = GameObject.Find("MapCounter");
        scoreCounter = GameObject.Find("ScoreText");
        keyCounter = GameObject.Find("KeyCounter");
        deathBar = GameObject.Find("DeathBar");
    }
    void UpdateUI()
    {
        // Map collectible
        if (mapCounter != null)
        {
            TextMeshProUGUI mapText = mapCounter.GetComponent<TextMeshProUGUI>();
            mapText.text = mapPieces.ToString();
            if (mapPieces == 4)
            {
                mapFound = true;
            }
        }

        // Score
        if (scoreCounter != null)
        {
            TextMeshProUGUI scoreText = scoreCounter.GetComponent<TextMeshProUGUI>();
            scoreText.text = score.ToString();
        }

        // Key collectible
        if (keyCounter != null)
        {
            if (keyFound)
            {
                keyCounter.transform.GetChild(0).gameObject.SetActive(false);
                keyCounter.transform.GetChild(1).gameObject.SetActive(true);
            } else
            {
                keyCounter.transform.GetChild(0).gameObject.SetActive(true);
                keyCounter.transform.GetChild(1).gameObject.SetActive(false);
            }
        }

        // Death bar
        if (deathBar != null)
        {
            if (lives == 3)
            {
                deathBar.transform.GetChild(3).gameObject.SetActive(true);
            }
            if (lives == 2)
            {
                deathBar.transform.GetChild(4).gameObject.SetActive(true);
                deathBar.transform.GetChild(3).gameObject.SetActive(false);
            }

            if (lives == 1)
            {
                deathBar.transform.GetChild(5).gameObject.SetActive(true);
                deathBar.transform.GetChild(4).gameObject.SetActive(false);
                deathBar.transform.GetChild(3).gameObject.SetActive(false);
            }

            if (lives == 0)
            {
                deathBar.transform.GetChild(5).gameObject.SetActive(false);
                deathBar.transform.GetChild(4).gameObject.SetActive(false);
                deathBar.transform.GetChild(3).gameObject.SetActive(false);
            }
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

    // ─── Collectibles ───────────────────────────────────────────────────────────
    public void MarkCollected(string id)
    {
        if (!collectedItems.Contains(id))
            collectedItems.Add(id);
    }

    public bool IsCollected(string id)
    {
        return collectedItems.Contains(id);
    }

    // ─── Score ───────────────────────────────────────────────────────────
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    public int GetScore() => score;

    // ─── Lives ───────────────────────────────────────────────────────────
    public void ProcessPlayerDeath()
    {
        lives--;
        if (lives > 0)
            ReloadCurrentScene();
        else
            LoadGameOver();
    }

    public int CheckLives()
    {
        return lives;
    }

    public void AddLife()
    {
        lives += 1;
        UpdateUI();
    }

    // ─── Scene Loading ───────────────────────────────────────────────────
    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadGameOver()
    {
        FindFirstObjectByType<PauseGameUI>().ShowLose();
    }

    public void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        //Debug.Log(nextIndex);
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
        else
            FindFirstObjectByType<PauseGameUI>().ShowWin();
    }
    // ─── Reset Game Session ───────────────────────────────────────────────────

    public void ResetSession()
    {
        lives = 3;
        score = 0;
        mapPieces = 0;
        mapFound = false;
        keyFound = false;
        collectedItems.Clear();

        UpdateUI();
    }
}