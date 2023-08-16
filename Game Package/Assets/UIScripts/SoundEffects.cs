using UnityEngine;
using UnityEngine.UI;

public class SoundEffects : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    public void PlaySound()
    {
        audioSource.Play();
    }
}
