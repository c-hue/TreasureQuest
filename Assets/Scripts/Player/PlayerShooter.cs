using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [Header("Melee")]
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float attackRate = 0.3f;
    [SerializeField] float attackRange = 0.5f;

    [Header("Shooting")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireRate = 0.3f;

    SpriteRenderer spriteRenderer;
    float nextFireTime;
    float nextAttackTime;
    Animator animator;

    // -- Player input ---------------------------------------
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        // Left click - close range attack
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextAttackTime)
        {
            animator.SetTrigger("attack");
            nextAttackTime = Time.time + attackRate;
        }

        // Right click - far range attack
        if (Mouse.current.rightButton.wasPressedThisFrame && Time.time >= nextFireTime)
        {
            animator.SetTrigger("shoot");
            nextFireTime = Time.time + fireRate;
        }
    }

    // -- Player attacks -------------------------------------
    public void meleeAttack()
    {
        // Determine direction based on which way the player is facing
        float direction = spriteRenderer.flipX ? -1f : 1f;

        // Detect enemies in range of attack
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Damage enemies
        foreach (Collider2D hit in hits)
        {
            EnemyPatrol patrolEnemy = hit.GetComponent<EnemyPatrol>();
            if (patrolEnemy != null)
            {
                patrolEnemy.Hit(transform.position.x);
                continue;
            }

            EnemyJump jumpEnemy = hit.GetComponent<EnemyJump>();
            if (jumpEnemy != null)
            {
                jumpEnemy.Hit();
            }
        }
    }
    public void throwSword()
    {
        // Determine direction based on which way the player is facing
        float direction = spriteRenderer.flipX ? -1f : 1f;

        // Spawn sword to be thrown
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);
        arrow.GetComponent<Arrow>().SetDirection(direction);
    }
}