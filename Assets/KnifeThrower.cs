using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeThrower : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Knife knifePrefab;
    private Knife currentKnife;

    void Start()
    {
        SpawnKnife();
    }

    void Update()
    {
        if (Pointer.current == null || !Pointer.current.press.wasPressedThisFrame) return;
        if (currentKnife == null || !gameManager.CanThrow) return;

        gameManager.RegisterThrow();
        currentKnife.Throw();
        currentKnife = null;

        if (gameManager.KnivesLeft > 0)
        {
            Invoke(nameof(SpawnKnife), 0.15f);
        }
    }

   void SpawnKnife()
    {
        Vector3 startPos = transform.position + Vector3.down * 1.5f;
        currentKnife = Instantiate(knifePrefab, startPos, Quaternion.identity);
        currentKnife.transform.DOMove(transform.position, 0.15f).SetEase(Ease.OutQuad);
    }
}
