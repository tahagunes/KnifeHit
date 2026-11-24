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
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ParticleSystem breakParticles;

    void OnEnable()
    {
        Knife.Stuck += OnKnifeStuck;
        Knife.Failed += OnKnifeFailed;
        gameManager.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        Knife.Stuck -= OnKnifeStuck;
        Knife.Failed -= OnKnifeFailed;
        gameManager.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
{
    if (state != GameState.Won) return;

    Knife[] stuckKnives = target.GetComponentsInChildren<Knife>();
    foreach (Knife knife in stuckKnives)
    {
        knife.FlyOff(target.position);
    }

    breakParticles.transform.position = target.position;
    breakParticles.Play();

    target.gameObject.SetActive(false);
}

    private void OnKnifeStuck(Vector2 hitPoint)
    {
        target.DOComplete();
        target.DOPunchScale(Vector3.one * hitPunch, hitDuration, 10, 1);

        hitParticles.transform.position = hitPoint;
        hitParticles.Play();
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