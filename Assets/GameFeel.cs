using DG.Tweening;
using UnityEngine;

public class GameFeel : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTransform;

    [Header("Kütük vuruş efekti")]
    [SerializeField] private float hitPunch = 0.3f;
    [SerializeField] private float hitDuration = 0.15f;

    [Header("Ekran sarsıntısı")]
    [SerializeField] private float shakeDuration = 0.3f;
    [SerializeField] private float shakeStrength = 0.3f;

    void OnEnable()
    {
        Knife.Stuck += OnKnifeStuck;
        Knife.Failed += OnKnifeFailed;
    }

    void OnDisable()
    {
        Knife.Stuck -= OnKnifeStuck;
        Knife.Failed -= OnKnifeFailed;
    }

    private void OnKnifeStuck()
    {
        target.DOComplete();
        target.DOPunchScale(Vector3.one * hitPunch, hitDuration, 10, 1);
    }

    private void OnKnifeFailed()
    {
        cameraTransform.DOComplete();
        cameraTransform.DOShakePosition(shakeDuration, shakeStrength, 20);

#if UNITY_ANDROID && !UNITY_EDITOR
        Handheld.Vibrate();
#endif
    }
}