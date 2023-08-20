using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class GameOverController : MonoBehaviour
{
    public Image rankImage; // Drag and drop the Image component where you want to display rank
    public TextMeshProUGUI scoreText; // Drag and drop the Text component where you want to display the score
    public TextMeshProUGUI highScoreText; // Drag and drop the Text component where you want to display the high score

    public Sprite rankS; // Assign the sprite for rank S in the inspector
    public Sprite rankA; // Assign the sprite for rank A in the inspector
    public Sprite rankB; // Assign the sprite for rank B in the inspector
    public Sprite rankC; // Assign the sprite for rank C in the inspector
    public Sprite rankD;
    public Sprite rankF;
    // ... add more as needed

    private void Start()
    {
        Gameplay gameplay = FindObjectOfType<Gameplay>();

        float totalAccuracy = (float)gameplay.TotalScore / gameplay.MaxScore;

        if (totalAccuracy >= 0.55) 
        {
            rankImage.sprite = rankS;
        }
        else if (totalAccuracy >= 0.45)
        {
            rankImage.sprite = rankA;
        }
        else if (totalAccuracy >= 0.35)
        {
            rankImage.sprite = rankB;
        }
        else if (totalAccuracy >= 0.25)
        {
            rankImage.sprite = rankC;
        }
        else if (totalAccuracy >= 0.15)
        {
            rankImage.sprite = rankD;
        }
        else
        {
            rankImage.sprite = rankF;
        }

        scoreText.text = "Score: " + gameplay.TotalScore.ToString();

        // Check and update high score
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0); // If "HighScore" doesn't exist, it defaults to 0
        if (gameplay.TotalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", gameplay.TotalScore); // Save the new high score
            PlayerPrefs.Save(); // Important to ensure that the data is saved to disk
        }

        // Display the high score
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0); // If "HighScore" doesn't exist, it defaults to 0
        highScoreText.text = "High Score: " + savedHighScore.ToString();

    }
}
