using UnityEngine;
public class Level3Exit : MonoBehaviour
{
    [SerializeField] float exitDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameSession.Instance?.enemiesDefeated == true)
        {
            if (other.CompareTag("Player"))
            {
                FindFirstObjectByType<PauseGameUI>().ShowDialogue(
                    "Well I'll be... the treasure at last. We chased it across sea an' storm, but at a heavy cost. I'll carry it home... for all who sailed with me.",
                    3
                );
                StartCoroutine(LoadNextLevel());
            }

        }
            
        // else 
        // FindFirstObjectByType<PauseGameUI>().ShowDialogue(
        //        "Hold fast... I won't be takin' the treasure while me crew still walks undead. Best put 'em to rest first.",
        //        3
        //    );
    }

    System.Collections.IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(exitDelay);
        GameSession.Instance?.LoadNextLevel();
    }
}