using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FeedbackManager : MonoBehaviour
{
    private TextMeshProUGUI feedbackText;
    private Vector3 initialScale;

    [Header("Scale Animation Settings")]
    [SerializeField] private float scaleMultiplier = 1.2f; // The factor by which the text will be scaled.
    [SerializeField] private float animationDuration = 0.5f; // How long the scaling animation will last.

    private void Awake()
    {
        feedbackText = GetComponent<TextMeshProUGUI>();
        initialScale = feedbackText.transform.localScale;
    }

    public void ShowFeedback(string message)
    {
        feedbackText.text = message;
        StartCoroutine(PlayScaleAnimation());
    }

    private IEnumerator PlayScaleAnimation()
    {
        float elapsed = 0f;
        Vector3 targetScale = initialScale * scaleMultiplier;

        // Scale up.
        while (elapsed < animationDuration / 2)
        {
            feedbackText.transform.localScale = Vector3.Lerp(initialScale, targetScale, elapsed / (animationDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        feedbackText.transform.localScale = targetScale;  // Ensure it's scaled up properly.
        elapsed = 0;  // Reset the elapsed time.

        // Scale down.
        while (elapsed < animationDuration / 2)
        {
            feedbackText.transform.localScale = Vector3.Lerp(targetScale, initialScale, elapsed / (animationDuration / 2));
            elapsed += Time.deltaTime;
            yield return null;
        }

        feedbackText.transform.localScale = initialScale;  // Reset to the initial scale.
    }
}
