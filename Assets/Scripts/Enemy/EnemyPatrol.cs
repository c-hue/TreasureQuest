using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    // ─── Settings ────────────────────────────────────────────────────────
    [Header("Patrol")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] Transform leftEdge;
    [SerializeField] Transform rightEdge;

    [Header("Ground/Wall Detection")]
    [SerializeField] Transform groundDetect;
    [SerializeField] float detectRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;

    [Header("Knockback")]
    [SerializeField] float kickX = 3f;
    [SerializeField] float kickY = 2f;

    // ─── State ───────────────────────────────────────────────────────────
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    int enemyHealth = 3;
    bool movingRight = true;
    bool isAlive = true;
    bool isHurt = false;
    float hitDirection;

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning",true);
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
        float direction = movingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        spriteRenderer.flipX = movingRight;
    }

    void CheckEdges()
    {
        // Turn around at patrol waypoints
        if (movingRight && transform.position.x >= rightEdge.position.x)
            TurnAround();
        else if (!movingRight && transform.position.x <= leftEdge.position.x)
            TurnAround();

        // Turn around at ledge (no ground detected below)
        bool groundAhead = Physics2D.OverlapCircle(groundDetect.position, detectRadius, groundLayer);
        if (!groundAhead)
            TurnAround();
    }
    void TurnAround()
    {
        movingRight = !movingRight;
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