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

    [Header("Death Knockback")]
    [SerializeField] float deathKickX = 1f;
    [SerializeField] float deathKickY = 1f;
    [SerializeField] float destroyDelay = 0.8f;


    // ─── State ───────────────────────────────────────────────────────────
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    Animator animator;
    int enemyHealth = 3;
    bool movingRight = true;
    bool isAlive = true;

    // ─── Lifecycle ───────────────────────────────────────────────────────
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isAlive) return;
        CheckEdges();
    }

    void FixedUpdate()
    {
        if (!isAlive) return;
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
        if (enemyHealth > 1)
        {
            enemyHealth--;
            animator.SetTrigger("hit");
        } else
        {
            isAlive = false;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;
            float knockDir = transform.position.x < hitDir ? -1f :1f;
            rb.AddForce(new Vector2(knockDir * deathKickX, deathKickY), ForceMode2D.Impulse);
            GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("die");
            Destroy(gameObject, 0.8f);
        }
    }
}