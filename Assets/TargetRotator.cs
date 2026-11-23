using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 150f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
    }
}