using UnityEngine;
using System.Collections;


public class EnemyJump : MonoBehaviour
{
    // ─── Settings ────────────────────────────────────────────────────────
    [Header("Patrol")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] Transform topEdge;
    [SerializeField] Transform bottomEdge;  

    // ─── State ───────────────────────────────────────────────────────────
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    int enemyHealth = 3;
    bool movingUp = true;
    bool isAlive = true;
    bool isHurt = false;
    float hitDirection;
    bool isFalling = false;
    bool isWaiting = false;
    GameObject enemyTracker;

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("isJumping", movingUp);
        enemyTracker = GameObject.Find("EnemyCounter");
    }

    void Update()
    {
        if (!isAlive || isHurt) return;
        CheckEdges();
    }

    void FixedUpdate()
    {
        if (!isAlive || isHurt) return;
        Patrol();
    }

    // ─── Patrol Logic ────────────────────────────────────────────────────
    void Patrol()
    {
        float direction = movingUp ? 1f : -1f;
        if (movingUp)
        {
            rb.linearVelocity = new Vector2(0, direction * moveSpeed);    

        } else
        {
            rb.linearVelocity = new Vector2(0, direction * 5f);    
        }
    }

    void CheckEdges()
    {
        // Turn around at patrol waypoints
        if (movingUp && transform.position.y >= topEdge.position.y)
        {
            reachedTop();
        }
        else if (!movingUp && transform.position.y <= bottomEdge.position.y)
        {
            reachedBottom();
        }
    }

    void reachedTop()
    {
        movingUp = !movingUp;
        animator.SetBool("isJumping", movingUp);
    }

    void reachedBottom()
    {
        if (isWaiting) return;

        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isWaiting", isWaiting);
        StartCoroutine(WaitingTimer());
    }

    IEnumerator WaitingTimer()
    {
        yield return new WaitForSeconds(3f);
        movingUp = true;
        isWaiting = false;
        animator.SetBool("isWaiting", isWaiting);
        animator.SetBool("isJumping", movingUp);
    }

    // ─── Death ───────────────────────────────────────────────────────────

    public void Hit()
    {
        if (!isAlive) return;

        if (enemyHealth > 1)
        {
            enemyHealth--;
            isHurt = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("hit");
        } else
        {
            isAlive = false;
            GameSession.Instance?.AddScore(500);
            enemyDefeated();
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("die");
        }
    }

    public void EndHit()
    {
        isHurt = false;
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