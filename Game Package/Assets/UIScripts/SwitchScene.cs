using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Import the UI namespace
using UnityEngine.EventSystems;


public class SwitchScene : MonoBehaviour
{
    public Button homeButton;  // Public reference to the UI button

    private string currentSceneName;


    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;

        if (homeButton == null)
        {
            // Try to find the button in the children of this object if not set
            homeButton = GetComponentInChildren<Button>();
        }

        // If the button is found, add a click listener to it
        if (homeButton != null)
        {
            homeButton.onClick.AddListener(GoToMainMenu);
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SongSelectionMenu");
        }

        // if (Input.GetMouseButtonDown(0) && currentSceneName == "GameOver")
        // {
        //     SceneManager.LoadScene("SongSelectionMenu");
        // }
    }

    // This method will be triggered when the home button is clicked
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
