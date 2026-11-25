using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Apply();
    }

    void Update()
    {
        if (Screen.safeArea != lastSafeArea)
            Apply();
    }

    private void Apply()
    {
        Rect safe = Screen.safeArea;
        lastSafeArea = safe;

        Vector2 min = safe.position;
        Vector2 max = safe.position + safe.size;
        min.x /= Screen.width;
        min.y /= Screen.height;
        max.x /= Screen.width;
        max.y /= Screen.height;

        rectTransform.anchorMin = min;
        rectTransform.anchorMax = max;
    }
}
