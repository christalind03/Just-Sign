using UnityEngine;
using UnityEngine.UI;

public class SearchFunctionality : MonoBehaviour
{
    public Button searchButton; // Drag your search button here
    public GameObject searchPanel; // Drag your search panel here
    public InputField searchInputField; // Drag your input field here
    public Transform resultsContainer; // The container where you'll populate the results

    private void Start()
    {
        // Initially, let's assume the search panel is hidden
        searchPanel.SetActive(false);

        // Attach the event listener
        searchButton.onClick.AddListener(ToggleSearchPanel);
        
        // Attach input field event
        searchInputField.onValueChanged.AddListener(UpdateSearchResults);
    }

    void ToggleSearchPanel()
    {
        bool isActive = searchPanel.activeSelf;
        searchPanel.SetActive(!isActive);
    }

    void UpdateSearchResults(string query)
    {
        // For now, just print the query
        // Later, you can add logic to populate the results based on the query
        Debug.Log("Search for: " + query);

        // Example: Let's just clear and add text based on the search query
        // (Note: This is basic and doesn't actually search anything. You'll need to implement actual search logic.)
        foreach (Transform child in resultsContainer)
        {
            Destroy(child.gameObject);
        }

        GameObject result = new GameObject("Result");
        Text resultText = result.AddComponent<Text>();
        resultText.text = "Searched for: " + query;
        result.transform.SetParent(resultsContainer, false);
    }
}
