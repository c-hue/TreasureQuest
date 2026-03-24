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
                GameSession.Instance?.AddScore(100);
            } 

            if (name == "Key")
            {
                GameSession.Instance?.KeyFound();
            }

            if (tag == "Coin")
            {
                switch(name)
                {
                    case "BlueDia":
                        pointValue = 1000;
                        break;
                    case "GreenDia":
                        pointValue = 500;
                        break;
                    case "GoldCoin":
                        pointValue = 100;
                        break;
                    case "SilverCoin":
                        pointValue = 50;
                        break;
                }
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