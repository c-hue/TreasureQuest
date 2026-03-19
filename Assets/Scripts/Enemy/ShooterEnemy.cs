using UnityEngine;

public class ShooterEnemy : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectilePos;
    [SerializeField] float shootInterval = 2f;

    [Header("Detection")]
    [SerializeField] float detectionRange = 15f;

    private Animator animator;
    private GameObject player;
    private float shootTimer;
    private bool isShooting;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
       //Debug.Log(distance);

        if (distance < detectionRange)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                shootTimer = 0f;
                Shoot();
            }
        }
        else
        {
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        animator.SetTrigger("Shoot");
    }

    public void SpawnProjectile()
    {
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
    }
}
