using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float pressedScale = 0.92f;

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(pressedScale, 0.08f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
    }
}
