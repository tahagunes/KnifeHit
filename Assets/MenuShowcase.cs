using DG.Tweening;
using UnityEngine;

// Idle animations that make the main menu feel alive.
public class MenuShowcase : MonoBehaviour
{
    [SerializeField] private RectTransform log;
    [SerializeField] private RectTransform title;
    [SerializeField] private Transform playButton;

    [SerializeField] private float logTurnDuration = 8f;
    [SerializeField] private float titleFloat = 18f;
    [SerializeField] private float playPulseScale = 1.06f;

    void Start()
    {
        // Log keeps turning, like in the game.
        log.DORotate(new Vector3(0f, 0f, -360f), logTurnDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1)
            .SetLink(log.gameObject);

        // Title gently floats up and down.
        title.DOAnchorPosY(title.anchoredPosition.y + titleFloat, 1.6f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(title.gameObject);

        // Play button "breathes" to draw the thumb.
        playButton.DOScale(playPulseScale, 0.8f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetLink(playButton.gameObject);
    }
}
