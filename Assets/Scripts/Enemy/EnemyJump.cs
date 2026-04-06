using UnityEngine;
using System.Collections;


public class EnemyJump : MonoBehaviour
{
    // ─── Settings ────────────────────────────────────────────────────────
    [Header("Patrol")]
    [SerializeField] float jumpForce = 30f;
    [SerializeField] Transform bottomEdge;  

    // ─── State ───────────────────────────────────────────────────────────
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    int enemyHealth = 3;
    bool isAlive = true;
    bool isHurt = false;
    bool isWaiting = false;
    GameObject enemyTracker;

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyTracker = GameObject.Find("EnemyCounter");

        animator.SetBool("isWaiting", true);
        animator.SetBool("isJumping", false);
    }

    void Update()
    {
        if (!isAlive || isHurt) return;
        CheckEdges();
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        if (isWaiting)
        {
            animator.SetBool("isWaiting", isWaiting);
            animator.SetBool("isJumping", false);
            return;
        }
        animator.SetBool("isWaiting", isWaiting);

        if (rb.linearVelocity.y > 0.1f)
        {
            animator.SetBool("isJumping", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
        }
    }

    // ─── Jump Logic ────────────────────────────────────────────────────
    void CheckEdges()
    {
        // When enemy comes back down to bottom
        if (!isWaiting && rb.linearVelocity.y <= 0 && transform.position.y <= bottomEdge.position.y)
        {
            reachedBottom();
        }
    }

    void reachedBottom()
    {
        if (isWaiting) return;

        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isWaiting", isWaiting);
        animator.SetBool("isJumping", false);
        StartCoroutine(WaitingTimer());
    }

    IEnumerator WaitingTimer()
    {
        yield return new WaitForSeconds(2f);
        isWaiting = false;
        animator.SetBool("isWaiting", isWaiting);

        rb.linearVelocity = new Vector2(0f, jumpForce);
    }

    // ─── Death ───────────────────────────────────────────────────────────

    public void Hit()
    {
        if (!isAlive) return;

        if (enemyHealth > 1)
        {
            enemyHealth--;
            isHurt = true;
            animator.SetTrigger("hit");
            rb.linearVelocity = Vector2.zero;
            string hurtSound = name + "Hurt";
            AudioManager.Instance?.PlayOneShot(hurtSound, this.transform.position);
        } else
        {
            isAlive = false;
            GameSession.Instance?.AddScore(500);
            enemyDefeated();
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("die");
            string deathSound = name + "Death";
            AudioManager.Instance?.PlayOneShot(deathSound, this.transform.position);
        }
    }

    public void EndHit()
    {
        isHurt = false;
        UpdateAnimations();
    }

    void enemyDefeated()
    {
        if (enemyTracker != null)
        {
            GameSession.Instance?.EnemyDefeated();
        }
        
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}