using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GameOverController : MonoBehaviour
{
    public Image rankImage; // Drag and drop the Image component where you want to display rank
    public TextMeshProUGUI scoreText; // Drag and drop the Text component where you want to display the score

    public Sprite rankS; // Assign the sprite for rank S in the inspector
    public Sprite rankA; // Assign the sprite for rank A in the inspector
    public Sprite rankB; // Assign the sprite for rank B in the inspector
    public Sprite rankC; // Assign the sprite for rank C in the inspector
    // ... add more as needed

    int totalScore = 90; // default value

    private void Start()
    {
        try
        {
            totalScore = FindObjectOfType<Gameplay>().GetTotalScore();
        }
        catch (Exception e)
        {
            Debug.LogWarning("Could not retrieve total score from Gameplay. Using default value. Error: " + e.Message);
        }

        if (totalScore >= 90) 
        {
            rankImage.sprite = rankS;
        }
        else if (totalScore >= 80)
        {
            rankImage.sprite = rankA;
        }
        else if (totalScore >= 60)
        {
            rankImage.sprite = rankB;
        }
        else
        {
            rankImage.sprite = rankC;
        }

        scoreText.text = "Score: " + totalScore.ToString();

    }
}
