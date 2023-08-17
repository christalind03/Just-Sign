using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;      // Reference to the settings panel
    public Button settingsButton;         // Reference to the settings button in the song selection
    public Button returnToMenuButton;     // Reference to the return button in the settings panel
    public Slider volumeSlider;           // Reference to the volume slider in the settings panel
    public Slider MainSlider;
    private AudioSource audioSource;

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

        // Find the AudioManager object and get its AudioSource component
        GameObject audioManager = GameObject.Find("AudioManager");
        if (audioManager)
        {
            audioSource = audioManager.GetComponent<AudioSource>();
            
            // Initialize the slider's value to the current volume of the audio source
            volumeSlider.value = audioSource.volume;
            MainSlider.value = audioSource.volume;

            // Add a listener to the slider to adjust volume in real-time
            volumeSlider.onValueChanged.AddListener(AdjustVolume);
            MainSlider.onValueChanged.AddListener(AdjustVolume);
        }
    }

    // Adjust the volume of the audio source
    public void AdjustVolume(float volume)
    {
        if (audioSource)
        {
            audioSource.volume = volume;
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
