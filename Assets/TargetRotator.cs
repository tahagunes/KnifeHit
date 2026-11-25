using UnityEngine;

public class TargetRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private float waveFrequency = 1.5f;
    public float CurrentSpeed { get; private set; }

    private Rigidbody2D rb;
    private bool isIrregular;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Configure(float speed, bool irregular)
    {
        rotationSpeed = speed;
        isIrregular = irregular;
    }

    void FixedUpdate()
    {
        float currentSpeed = rotationSpeed;

        if (isIrregular)
        {
            currentSpeed = rotationSpeed * Mathf.Sin(Time.time * waveFrequency) * 1.5f;
        }

        CurrentSpeed = currentSpeed;

        rb.MoveRotation(rb.rotation + currentSpeed * Time.fixedDeltaTime);
    }
}