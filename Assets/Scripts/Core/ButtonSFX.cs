using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void PlaySFX()
    {
        AudioManager.Instance?.PlayOneShot("buttonClick", this.transform.position);
    }
}
