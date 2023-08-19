using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource backgroundMusic;
    public AudioClip[] sfxClips;  // All the sound effects clips you need

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Listen to scene changes
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Start playing music immediately
            backgroundMusic.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the loaded scene is the main menu or the game over scene
        if (scene.name == "MainMenu" || scene.name == "GameOver")
        {
            Debug.Log("Music playing");
            if (!backgroundMusic.isPlaying) // Added a check to prevent replaying
            {
                backgroundMusic.Play();
            }
        }
        else if (scene.name == "Gameplay")
        {
            Debug.Log("Music stopped");
            backgroundMusic.Stop();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume;
    }

    public void PlaySFX(int clipIndex) 
    {
        if (clipIndex >= 0 && clipIndex < sfxClips.Length)
        {
            backgroundMusic.PlayOneShot(sfxClips[clipIndex]);
        }
    }
}
