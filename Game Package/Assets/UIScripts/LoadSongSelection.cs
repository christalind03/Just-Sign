using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSongSelection : MonoBehaviour
{
    public void LoadSongSelectionMenu()
    {
        SceneManager.LoadScene("SongSelectionMenu");
    }
}
