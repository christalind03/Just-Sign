using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPanel;      // Reference to the settings panel
    public Button settingsButton;         // Reference to the settings button in the song selection
    public Button returnToMenuButton;     // Reference to the return button in the settings panel
    
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
