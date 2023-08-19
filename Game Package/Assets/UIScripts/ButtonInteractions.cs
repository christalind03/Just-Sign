using UnityEngine;

public class ButtonInteractions : MonoBehaviour
{
    public void PlayButtonSFX(int clipIndex)
    {
        AudioManager.Instance.PlaySFX(clipIndex);
    }

}
