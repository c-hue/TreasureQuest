using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance?.PlayMusic("MenuMusic");
    }
    
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void HowToPlay()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    
}
