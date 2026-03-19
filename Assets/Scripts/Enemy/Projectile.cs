using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float speed = 6f;
    [SerializeField] float direction = 1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            animator.SetTrigger("Hit");
            speed = 0f;
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
