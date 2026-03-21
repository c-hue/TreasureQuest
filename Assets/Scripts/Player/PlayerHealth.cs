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
    int counter = 0;

    PlayerMovement movement;
    SpriteRenderer spriteRenderer;

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        currentHealth = maxHealth;
        movement = GetComponent<PlayerMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        HandleInvincibility();
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
        if (counter == 0)
        {
            barHealth[counter].SetActive(false);
            counter++;
        } else if (counter == 1)
        {
            barHealth[counter].SetActive(false);
            counter++;
        } else if (counter == 2)
        {
            barHealth[counter].SetActive(false);
            counter++;
        } else
        {
            Debug.Log("Die");
        }
    }
}