using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameLoader : MonoBehaviour
{
    private string allSongFolder = $"{Directory.GetCurrentDirectory()}/Assets/Songs/";
    public static string selectedSongPath;
    private Gameplay _gameplay;

    public void Start(){
        _gameplay = GameObject.Find("GamePlayManager").GetComponent<Gameplay>();
    }

    public void LoadScene(string songPath)
    {
        selectedSongPath = allSongFolder + songPath;
        _gameplay.LoadSong(selectedSongPath);
        _gameplay.StartGame();
        SceneManager.LoadScene("Gameplay");
    }
}
