using UnityEngine;
public class Level1Exit : MonoBehaviour
{
    [SerializeField] float exitDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameSession.Instance?.mapFound == true)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(LoadNextLevel());
            }
        }
        
    }

    System.Collections.IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(exitDelay);
        GameSession.Instance?.LoadNextLevel();
    }
}