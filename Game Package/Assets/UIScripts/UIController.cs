using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button Song1;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Song1 = root.Q<Button>("Song1");
        Song1.clicked += Song1ButtonPressed;
    }

    void Song1ButtonPressed(){
        SceneManager.LoadScene("Gameplay");
    }
}
