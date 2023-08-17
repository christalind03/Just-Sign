using UnityEngine;
using UnityEngine.UI;

public class SoundEffects : MonoBehaviour
{
    public AudioSource buttonSFX;

    public void playSoundEffect()
    {
        buttonSFX.Play();
    }
}
