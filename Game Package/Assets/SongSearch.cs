using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SongSearch : MonoBehaviour
{
    public TMP_InputField searchInputField; 
    public List<Button> songButtons = new List<Button>();
    private List<Vector3> originalLocalPositions = new List<Vector3>();
    public GameObject searchPanel; // Add this for the search panel reference

    private void Start()
    {
        foreach (Button button in songButtons)
        {
            originalLocalPositions.Add(button.transform.localPosition);
        }

        // Initially hide the search panel
        searchPanel.SetActive(false);
        
        // Attach input field event
        searchInputField.onEndEdit.AddListener(FilterSongList);
    }

    // Add this method to toggle the search panel
    public void ToggleSearchPanel()
    {
        searchPanel.SetActive(!searchPanel.activeSelf);
    }

    void FilterSongList(string query)
    {
        if (string.IsNullOrEmpty(query.Trim())) // If the query is empty
        {
            for (int i = 0; i < songButtons.Count; i++)
            {
                songButtons[i].gameObject.SetActive(true); // Show every button
                songButtons[i].transform.localPosition = originalLocalPositions[i];
            }
            return;  // Exit the function early
        }

        int activeButtonIndex = 0;
        for (int i = 0; i < songButtons.Count; i++)
        {
            Button button = songButtons[i];
            if (button.gameObject.name.ToLower().Contains(query.ToLower()))
            {
                button.gameObject.SetActive(true);

                // Reposition the button
                Vector3 newPosition = button.transform.parent.GetChild(0).transform.position;
                newPosition += activeButtonIndex * new Vector3(button.GetComponent<RectTransform>().rect.width + 0.5f, 0, 0);
                button.transform.position = newPosition;

                // Increment the index for the next match
                activeButtonIndex++;
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }






}
