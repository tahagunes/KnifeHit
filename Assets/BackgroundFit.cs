using UnityEngine;

// Scales a sprite uniformly so it always covers the whole camera view (no empty edges on tablets).
[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFit : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float lastAspect;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Fit();
    }

    void Update()
    {
        if (!Mathf.Approximately(Camera.main.aspect, lastAspect))
            Fit();
    }

    private void Fit()
    {
        Camera cam = Camera.main;
        lastAspect = cam.aspect;

        float viewHeight = cam.orthographicSize * 2f;
        float viewWidth = viewHeight * cam.aspect;
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scale = Mathf.Max(viewWidth / spriteSize.x, viewHeight / spriteSize.y);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
