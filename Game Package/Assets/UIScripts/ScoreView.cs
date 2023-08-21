using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreView : MonoBehaviour
{
    public Image highestRankImage; // Drag and drop the Image component where you want to display highest rank
    public TextMeshProUGUI highestRankText; // If you still want to show rank as text, keep this. Otherwise, you can remove this.

    public Sprite rankS; // Assign the sprite for rank S in the inspector
    public Sprite rankA; // Assign the sprite for rank A in the inspector
    public Sprite rankB; // Assign the sprite for rank B in the inspector
    public Sprite rankC; // Assign the sprite for rank C in the inspector
    public Sprite rankD;
    public Sprite rankF;

    private void Start()
    {
        DisplayHighestRank();
    }

    private void DisplayHighestRank()
    {
        string highestRank = PlayerPrefs.GetString("HighestRank", "F"); // Defaults to "F" if not found

        // Update rank text (you can remove this if you only want the sprite)
        highestRankText.text = $"Score: {highestRank}";

        // Set the highest rank sprite
        SetRankSprite(highestRank);
    }

    private void SetRankSprite(string rank)
    {
        switch(rank)
        {
            case "S":
                highestRankImage.sprite = rankS;
                break;
            case "A":
                highestRankImage.sprite = rankA;
                break;
            case "B":
                highestRankImage.sprite = rankB;
                break;
            case "C":
                highestRankImage.sprite = rankC;
                break;
            case "D":
                highestRankImage.sprite = rankD;
                break;
            default:
                highestRankImage.sprite = rankF;
                break;
        }
    }
}
