using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    // ─── Settings ────────────────────────────────────────────────────────
    [Header("Health")]
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invincibilityDuration = 1.5f;

    [Header("UI")]
    [SerializeField] GameObject[] barHealth;      // Array of Bar Health

    [Header("Effects")]
    [SerializeField] GameObject deathParticles;

    // ─── State ───────────────────────────────────────────────────────────
    int currentHealth;
    bool isInvincible;
    float invincibilityTimer;
    PlayerMovement movement;
    SpriteRenderer spriteRenderer;
    GameObject healthBar;

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBar = GameObject.Find("HealthBar");
    }

    void Update()
    {
        HandleInvincibility();
        // Debug.Log(currentHealth);
    }

    // ─── Invincibility Flash ─────────────────────────────────────────────
    void HandleInvincibility()
    {
        if (!isInvincible) return;

        invincibilityTimer -= Time.deltaTime;
        // Flash effect: toggle sprite visibility
        spriteRenderer.enabled = Mathf.Sin(invincibilityTimer * 20f) > 0;

        if (invincibilityTimer <= 0)
        {
            isInvincible = false;
            spriteRenderer.enabled = true;
        }
    }

    // ─── Public API ──────────────────────────────────────────────────────
    public void TakeDamage(int amount)
    {
        if (isInvincible) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateBarUI();
        CameraShake.Instance?.Shake(2f, 0.3f);

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Start invincibility window
        isInvincible = true;
        invincibilityTimer = invincibilityDuration;
    }

    public int CheckHealth() {
        return currentHealth;
    }

    public void Heal() {
        currentHealth += 1;
        UpdateBarUI();
    }

    void Die()
    {
        if (deathParticles != null)
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        
        movement.onDeath();
    }

    public void GameOver()
    {
        FindFirstObjectByType<GameSession>()?.ProcessPlayerDeath();
    }

    // ─── UI ──────────────────────────────────────────────────────────────
    void UpdateBarUI()
    {
        if (currentHealth == 3)
        {
            healthBar.transform.GetChild(3).gameObject.SetActive(true);
        }
        if (currentHealth == 2)
        {
            healthBar.transform.GetChild(4).gameObject.SetActive(true);
            healthBar.transform.GetChild(3).gameObject.SetActive(false);
        }

        if (currentHealth == 1)
        {
            healthBar.transform.GetChild(5).gameObject.SetActive(true);
            healthBar.transform.GetChild(4).gameObject.SetActive(false);
        }

        if (currentHealth == 0)
        {
            healthBar.transform.GetChild(5).gameObject.SetActive(false);
        }
    }
}