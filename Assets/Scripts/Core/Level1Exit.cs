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
                FindFirstObjectByType<PauseGameUI>().ShowDialogue(
                "Yo ho ho! The map be pointin' to an underground cave. Looks like I'll be headin' down there next to find 'em.",
                1,
                "Lvl1End"
            );

                StartCoroutine(LoadNextLevel());
            }
        }

        else
        {
            FindFirstObjectByType<PauseGameUI>().ShowDialogue(
                "Best be gatherin' all the missin' map pieces afore I wander any further. Wouldn't want to get meself lost out here like me crew!",
                1,
                "Lvl1Pause"
            );
        }
        
    }

    System.Collections.IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(exitDelay);
        GameSession.Instance?.LoadNextLevel();
    }
}