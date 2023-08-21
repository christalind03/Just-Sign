using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;      // Reference to the settings panel
    public Button settingsButton;         // Reference to the settings button in the song selection
    public Button returnToMenuButton;     // Reference to the return button in the settings panel
    public Slider volumeSlider;           // Reference to the BGM volume slider in the settings panel
    public Slider sfxVolumeSlider;        // Reference to the SFX volume slider in the settings panel
    public Slider MainSlider;             // Not sure what this slider does, retaining it for now
    private AudioSource audioSource;      // For BGM
    private AudioSource sfxAudioSource;   // For SFX

    private void Start()
    {
        // Initially hide the settings panel
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        // Attach event listener for settings button
        if (settingsButton != null)
        {
            settingsButton.onClick.AddListener(ShowSettingsPanel);
        }

        // Attach event listener for return button inside settings
        if (returnToMenuButton != null)
        {
            returnToMenuButton.onClick.AddListener(HideSettingsPanel);
        }

        // Find the AudioManager object and get its AudioSource components
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager)
        {
            audioSource = audioManager.GetComponent<AudioSource>();
            sfxAudioSource = audioManager.transform.Find("SFXAudioSource").GetComponent<AudioSource>();

            // Initialize the sliders' values to the current volumes of the audio sources
            volumeSlider.value = audioSource.volume;
            sfxVolumeSlider.value = sfxAudioSource.volume;
            MainSlider.value = audioSource.volume; // Retained this, but not sure of its function

            // Add listeners to the sliders to adjust volume in real-time
            volumeSlider.onValueChanged.AddListener(AdjustVolume);
            sfxVolumeSlider.onValueChanged.AddListener(AdjustSFXVolume);
            MainSlider.onValueChanged.AddListener(AdjustVolume); // Assuming MainSlider adjusts BGM
        }
    }

    // Adjust the volume of the BGM audio source
    public void AdjustVolume(float volume)
    {
        if (audioSource)
        {
            audioSource.volume = volume;
        }
    }

    // Adjust the volume of the SFX audio source
    public void AdjustSFXVolume(float volume)
    {
        if (sfxAudioSource)
        {
            sfxAudioSource.volume = volume;
        }
    }

    // Show the settings panel
    private void ShowSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    // Hide the settings panel
    private void HideSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }
}
