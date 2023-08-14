using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SongSearch : MonoBehaviour
{
    public TMP_InputField searchInputField; 
    public List<Button> songButtons = new List<Button>();
    public GameObject searchPanel; // Add this for the search panel reference

    private void Start()
    {
        // Initially hide the search panel
        searchPanel.SetActive(false);
        
        // Attach input field event
        searchInputField.onValueChanged.AddListener(FilterSongList);
    }

    // Add this method to toggle the search panel
    public void ToggleSearchPanel()
    {
        searchPanel.SetActive(!searchPanel.activeSelf);
    }

    void FilterSongList(string query)
    {
        foreach (Button button in songButtons)
        {
            // Using the button's name for filtering
            if (button.gameObject.name.ToLower().Contains(query.ToLower()))
            {
                button.gameObject.SetActive(true);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
