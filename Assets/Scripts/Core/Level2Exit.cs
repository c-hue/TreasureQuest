using UnityEngine;
public class Level2Exit : MonoBehaviour
{
    [SerializeField] float exitDelay = 1f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameSession.Instance?.keyFound == true)
        {
            if (other.CompareTag("Player"))
            {
                FindFirstObjectByType<PauseGameUI>().ShowDialogue(
                "Curse the seas! The ship be in ruins! I pray me crew made it out alive. Best I haul what's left o' her back to shore an' have a look inside.",
                0,
                "Lvl2End"
            );
                StartCoroutine(LoadNextLevel());
            }
        }
        else
        {
            FindFirstObjectByType<PauseGameUI>().ShowDialogue(
                "Blimey! The ship be locked tight. Mayhap me crew dropped the key somewhere 'round this cave...",
                2,
                "Lvl2Pause"
            );
        }
    }

    System.Collections.IEnumerator LoadNextLevel()
    {
        yield return new WaitForSeconds(exitDelay);
        GameSession.Instance?.LoadNextLevel();
    }
}