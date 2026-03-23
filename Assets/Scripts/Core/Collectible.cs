using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] int pointValue;
    [SerializeField] GameObject collectEffect;

    void Start()
    {
        string name = this.transform.name;
        string tag = this.transform.tag;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (name == "Map")
            {
                GameSession.Instance?.AddMap();
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
            if (collectEffect != null)
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}