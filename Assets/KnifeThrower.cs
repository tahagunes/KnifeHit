using UnityEngine;
using UnityEngine.InputSystem;

public class KnifeThrower : MonoBehaviour
{
    [SerializeField] private GameObject knifePrefab;
    private Knife currentKnife;

    void Start()
    {
        SpawnKnife();
    }

    void Update()
    {
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            Debug.Log("Touch detected!");
            if (currentKnife != null)
            {
                currentKnife.Throw();
                currentKnife = null;
                Invoke(nameof(SpawnKnife), 0.15f);
                Debug.Log("Knife thrown!");
            }
            else
            {
                Debug.Log("currentKnife is null!");
            }
        }
    }

    void SpawnKnife()
    {
        GameObject knifeInstance = Instantiate(knifePrefab, transform.position, Quaternion.identity);
        currentKnife = knifeInstance.GetComponent<Knife>();
    }
}
