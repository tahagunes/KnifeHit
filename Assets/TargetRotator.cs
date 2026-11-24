using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 150f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    void FixedUpdate()
    {
        rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
    }
}