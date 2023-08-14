using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SongSearch : MonoBehaviour
{
    public InputField searchInputField;
    public List<Button> songButtons = new List<Button>();

    private void Start()
    {
        // Attach input field event
        searchInputField.onValueChanged.AddListener(FilterSongList);
    }

    void FilterSongList(string query)
    {
        foreach (Button button in songButtons)
        {
            // Check if the button's label contains the search query
            Text buttonText = button.GetComponentInChildren<Text>();
            if (buttonText.text.ToLower().Contains(query.ToLower()))
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
