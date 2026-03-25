using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int pointValue;
    [SerializeField] GameObject collectEffect;
    Animator animator;

    void Start()
    {
        string name = this.transform.name;
        string tag = this.transform.tag;
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (name == "Map")
            {
                GameSession.Instance?.AddMap();
                GameSession.Instance?.AddScore(250);
            } 

            if (name == "Key")
            {
                GameSession.Instance?.KeyFound();
            }

            if (tag == "Coin")
            {
                GameSession.Instance?.AddScore(pointValue);
            }

            if (tag == "Potion")
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (name.StartsWith("Red") && playerHealth.CheckHealth() < 3)
                    playerHealth.Heal();
                else if (name.StartsWith("Green") && GameSession.Instance?.CheckLives() < 3)
                    GameSession.Instance?.AddLife();
                else
                    GameSession.Instance?.AddScore(pointValue);
            }        
            
            AudioManager.Instance?.PlayCollect();
            animator.SetTrigger("Collect");
            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);
        }
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}