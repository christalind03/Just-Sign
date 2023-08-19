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

        if (totalScore >= 9000) 
        {
            rankImage.sprite = rankS;
        }
        else if (totalScore >= 7000)
        {
            rankImage.sprite = rankA;
        }
        else if (totalScore >= 6000)
        {
            rankImage.sprite = rankB;
        }
        else if (totalScore >= 5000)
        {
            rankImage.sprite = rankC;
        }
        else if (totalScore >= 4000)
        {
            rankImage.sprite = rankD;
        }
        else
        {
            rankImage.sprite = rankF;
        }

        scoreText.text = "Score: " + totalScore.ToString();

        // Check and update high score
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0); // If "HighScore" doesn't exist, it defaults to 0
        if (totalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", totalScore); // Save the new high score
            PlayerPrefs.Save(); // Important to ensure that the data is saved to disk
        }

        // Display the high score
        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0); // If "HighScore" doesn't exist, it defaults to 0
        highScoreText.text = "High Score: " + savedHighScore.ToString();

    }
}
