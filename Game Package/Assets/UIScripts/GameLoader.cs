using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
