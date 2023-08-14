using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusic;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Start playing music immediately
            backgroundMusic.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume;
    }

    public void StopAndDestroyMusic()
    {
        backgroundMusic.Stop();
        Destroy(gameObject); // This will destroy the AudioManager and its AudioSource
    }
}
