using System;
using DG.Tweening;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public static event Action<int> Collected;

    [SerializeField] private int coinValue = 1;

    private bool isCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;

        Knife knife = other.GetComponent<Knife>();
        if (knife == null || !knife.IsFlying) return;

        isCollected = true;
        Collected?.Invoke(coinValue);

        transform.SetParent(null);
        GetComponent<Collider2D>().enabled = false;
        transform.DOScale(0f, 0.2f).SetEase(Ease.InBack);
        transform.DOMoveY(transform.position.y + 1f, 0.2f);
        Destroy(gameObject, 0.25f);
    }
}