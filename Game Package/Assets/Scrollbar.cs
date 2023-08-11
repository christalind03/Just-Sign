using UnityEngine;
using UnityEngine.UIElements;

public class StyleSheetAssigner : MonoBehaviour
{
    public StyleSheet customStyleSheet;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        
        // Add the style sheet
        root.styleSheets.Add(customStyleSheet);
    }
}
