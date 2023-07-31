using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateLogo : MonoBehaviour
{
    public float speed = 1f; // Speed of oscillation
    public float height = 0.5f; // Height of oscillation

    RectTransform rectTransform;
    Vector2 startPos;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    void Update()
    {
        Vector2 v = startPos;
        v.y += Mathf.Sin(Time.time * speed) * height;
        rectTransform.anchoredPosition = v;
    }
}
