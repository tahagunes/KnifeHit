using UnityEngine;
using System;
using DG.Tweening;

public class Knife : MonoBehaviour
{
    [SerializeField] private float throwSpeed = 20f;
    [SerializeField] private float stickDepth = 0.3f;

    private Rigidbody2D rb;
    private bool isThrown = false;
    private bool isStuck = false;
    private bool hasFailed = false;
    public static event Action Stuck;
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
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       if (isStuck || hasFailed) return;

        if (collision.gameObject.CompareTag("Target"))
        {
            isStuck = true;
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;

            Collider2D targetCollider = collision.collider;
            Vector2 center = targetCollider.bounds.center;
            float radius = targetCollider.bounds.extents.x;
            Vector2 outward = ((Vector2)transform.position - center).normalized;

            float halfLength = transform.lossyScale.y * 0.5f;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, -outward);
            transform.position = center + outward * (radius + halfLength - stickDepth);

            transform.SetParent(collision.transform, true);
            Stuck?.Invoke();
        }
        else if (collision.gameObject.CompareTag("Knife"))
        {
            Failed?.Invoke();
            Fail();
        }
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

        Debug.Log("Game Over");
    }
}