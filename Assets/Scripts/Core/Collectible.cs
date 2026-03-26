using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectible : MonoBehaviour
{
    [SerializeField] int pointValue;
    [SerializeField] GameObject collectEffect;
    Animator animator;
    string ID;

    void Start()
    {
        string name = this.transform.name;
        string tag = this.transform.tag;
        animator = GetComponent<Animator>();

        // Unique ID based on scene, object, and position
        Vector3 pos = transform.position;
        ID = SceneManager.GetActiveScene().name + "_" +
                gameObject.name + "_" +
                Mathf.RoundToInt(pos.x * 100f) + "_" +
                Mathf.RoundToInt(pos.y * 100f);

        // Ensure collectibles do not respawn in scene after player death
        if (GameSession.Instance != null && GameSession.Instance.IsCollected(ID))
            Destroy(gameObject);
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
                GameSession.Instance?.MarkCollected(ID);
                GameSession.Instance?.AddScore(pointValue);
            }

            if (tag == "Potion")
            {
                GameSession.Instance?.MarkCollected(ID);

                // Red potion heals, green potion adds life; if full, add score
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