using UnityEngine;
using System.Collections;


public class EnemyJump : MonoBehaviour
{
    // ─── Settings ────────────────────────────────────────────────────────
    [Header("Patrol")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Transform topEdge;
    [SerializeField] Transform bottomEdge;  

    [Header("Knockback")]
    [SerializeField] float kickX = 3f;
    [SerializeField] float kickY = 2f;

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

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("isJumping", movingUp);
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
        rb.linearVelocity = new Vector2(0, direction * moveSpeed);    
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

    public void Hit(float hitDir)
    {
        if (!isAlive) return;
        hitDirection = hitDir;

        if (enemyHealth > 1)
        {
            enemyHealth--;
            isHurt = true;
            rb.linearVelocity = Vector2.zero;
            animator.SetTrigger("hit");
        } else
        {
            isAlive = false;
            rb.linearVelocity = Vector2.zero;

            animator.SetTrigger("die");
        }
    }
    
    public void ApplyKnockback()
    {
        float knockDir = transform.position.x < hitDirection ? -1f : 1f;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(knockDir * kickX, kickY), ForceMode2D.Impulse);
    }

    public void EndHit()
    {
        isHurt = false;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}