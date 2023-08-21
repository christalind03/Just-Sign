using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Import the UI namespace
using UnityEngine.EventSystems;
using TMPro;

public class SwitchScene : MonoBehaviour
{
    // public Button homeButton; 
    public TextMeshProUGUI loadingTMPText; 

    private string currentSceneName;
    private UDP_Server _udpServer;

    private void Start()
    {
        _udpServer = UDP_Server.Instance;
        currentSceneName = SceneManager.GetActiveScene().name;

        // if (homeButton == null)
        // {
        //     // Try to find the button in the children of this object if not set
        //     homeButton = GetComponentInChildren<Button>();
        // }

        // // If the button is found, add a click listener to it
        // if (homeButton != null)
        // {
        //     homeButton.onClick.AddListener(GoToMainMenu);
        // }
    }


    void Update()
    {
        string clientMessage = _udpServer.ReceivedData;

        if (clientMessage == "CLIENT READY")
        {
            loadingTMPText.text = "Press Space to Start"; // Change the TextMeshPro text
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _udpServer.ReceivedData = null;
                SceneManager.LoadScene("SongSelectionMenu");
            }
        }
    }

    // This method will be triggered when the home button is clicked
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
