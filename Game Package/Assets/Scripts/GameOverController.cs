using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public Image rankImage;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    public Sprite rankS;
    public Sprite rankA;
    public Sprite rankB;
    public Sprite rankC;
    public Sprite rankD;
    public Sprite rankF;

    private void Start()
    {
        Gameplay gameplay = FindObjectOfType<Gameplay>();
        float totalAccuracy = (float)gameplay.TotalScore / gameplay.MaxScore;

        string currentRank = DetermineRank(totalAccuracy);
        SetRankSprite(currentRank);  // Set current playthrough rank
        
        scoreText.text = "Score: " + gameplay.TotalScore.ToString();

        // Check and update high score
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (gameplay.TotalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", gameplay.TotalScore);
        }

        // Check and update rank if it's better
        string highestRank = PlayerPrefs.GetString("HighestRank", "F");
        if (RankIsHigher(currentRank, highestRank))
        {
            PlayerPrefs.SetString("HighestRank", currentRank);
        }

        PlayerPrefs.Save();

        int savedHighScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + savedHighScore.ToString();
    }

    private string DetermineRank(float accuracy)
    {
        if (accuracy >= 0.55) return "S";
        if (accuracy >= 0.45) return "A";
        if (accuracy >= 0.35) return "B";
        if (accuracy >= 0.25) return "C";
        if (accuracy >= 0.15) return "D";
        return "F";
    }

    private bool RankIsHigher(string currentRank, string highestRank)
    {
        string[] rankOrder = { "F", "D", "C", "B", "A", "S" };
        int currentIndex = System.Array.IndexOf(rankOrder, currentRank);
        int highestIndex = System.Array.IndexOf(rankOrder, highestRank);

        return currentIndex > highestIndex;
    }

    private void SetRankSprite(string rank)
    {
        switch(rank)
        {
            case "S":
                rankImage.sprite = rankS;
                break;
            case "A":
                rankImage.sprite = rankA;
                break;
            case "B":
                rankImage.sprite = rankB;
                break;
            case "C":
                rankImage.sprite = rankC;
                break;
            case "D":
                rankImage.sprite = rankD;
                break;
            default:
                rankImage.sprite = rankF;
                break;
        }
    }
}
