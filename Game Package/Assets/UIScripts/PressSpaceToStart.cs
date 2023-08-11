using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    private string currentSceneName;

    private void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("SongSelectionMenu"); 
        }

        if (Input.GetMouseButtonDown(0) && currentSceneName == "GameOver")
        {
            // Load the SongSelectionMenu scene
            SceneManager.LoadScene("SongSelectionMenu");
        }
    }
}
