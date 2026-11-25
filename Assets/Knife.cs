using UnityEngine;
using System;
using DG.Tweening;

public class Knife : MonoBehaviour
{
    [SerializeField] private float throwSpeed = 20f;
    [SerializeField] private float stickDepth = 0.3f;
    public static event Action Thrown;
    public bool IsFlying => isThrown && !isStuck && !hasFailed;
    private Rigidbody2D rb;
    private bool isThrown = false;
    private bool isStuck = false;
    private bool hasFailed = false;
    public static event Action<Vector2> Stuck;
    public static event Action Failed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Throw()
    {
        transform.DOKill();
        if (!isThrown)
        {
            isThrown = true;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.up * throwSpeed;
            Thrown?.Invoke();
        }
    }

    public void FlyOff(Vector2 center)
    {
        transform.SetParent(null);
        GetComponent<Collider2D>().enabled = false;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 2f;

        Vector2 direction = ((Vector2)transform.position - center).normalized;
        rb.linearVelocity = direction * UnityEngine.Random.Range(4f, 7f) + Vector2.up * 3f;
        rb.angularVelocity = UnityEngine.Random.Range(-720f, 720f);

        Destroy(gameObject, 2f);
    }

    void OnCollisionEnter2D(Collision2D collision)
{
    if (isStuck || hasFailed) return;

    if (collision.gameObject.CompareTag("Target"))
    {
        Vector2 hitPoint = collision.GetContact(0).point;

        Collider2D targetCollider = collision.collider;
        Vector2 center = targetCollider.bounds.center;
        float radius = targetCollider.bounds.extents.x;
        Vector2 outward = ((Vector2)transform.position - center).normalized;

        StickTo(collision.transform, center, radius, outward);
        Stuck?.Invoke(hitPoint);
    }
    else if (collision.gameObject.CompareTag("Knife"))
    {
        Fail();
    }
}

public void PlaceOnTarget(Transform target, float angleDegrees)
{
    CircleCollider2D targetCollider = target.GetComponent<CircleCollider2D>();
    Vector2 center = target.position;
    float radius = targetCollider.radius * target.lossyScale.x;

    float angle = angleDegrees * Mathf.Deg2Rad;
    Vector2 outward = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

    StickTo(target, center, radius, outward);
}

private void StickTo(Transform target, Vector2 center, float radius, Vector2 outward)
{
    isStuck = true;
    rb.linearVelocity = Vector2.zero;
    rb.bodyType = RigidbodyType2D.Kinematic;

    float halfLength = GetComponent<BoxCollider2D>().size.y * transform.lossyScale.y * 0.5f;
    transform.rotation = Quaternion.FromToRotation(Vector3.up, -outward);
    transform.position = center + outward * (radius + halfLength - stickDepth);

    transform.SetParent(target, true);
}

    void Fail()
    {
        hasFailed = true;

        GetComponent<Collider2D>().enabled = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.gravityScale = 3f;
        rb.linearVelocity = new Vector2(UnityEngine.Random.Range(-3f, 3f), -5f);
        rb.angularVelocity = UnityEngine.Random.Range(-720f, 720f);

        Destroy(gameObject, 2f);

        Failed?.Invoke();
    }
}