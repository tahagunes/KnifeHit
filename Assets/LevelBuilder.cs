using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform target;
    [SerializeField] private Apple applePrefab;
    [SerializeField] private Knife knifePrefab;
    [SerializeField] private Color bossColor = new Color(0.5f, 0.1f, 0.1f);

    [Tooltip("Minimum angle between any two apples/obstacle knives, so a knife can never hide behind an apple.")]
    [SerializeField] private float minSeparation = 30f;
    [SerializeField] private int placementAttempts = 50;

    private readonly List<float> usedAngles = new List<float>();

    void Start()
    {
        var random = new System.Random(gameManager.CurrentLevelData.seed + 1000);
        if (gameManager.CurrentLevelData.isBoss)
        {
            target.GetComponent<SpriteRenderer>().color = bossColor;
            target.localScale *= 1.2f;
        }

        float radius = target.GetComponent<CircleCollider2D>().radius * target.lossyScale.x;

        int obstacleCount = gameManager.CurrentLevelData.obstacleKnives;
        for (int i = 0; i < obstacleCount; i++)
        {
            float angle = PickFreeAngle(random);
            Knife knife = Instantiate(knifePrefab);
            knife.PlaceOnTarget(target, angle);
        }

        int appleCount = gameManager.CurrentLevelData.appleCount;
        for (int i = 0; i < appleCount; i++)
        {
            float angle = PickFreeAngle(random) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector3 position = target.position + (Vector3)(direction * radius);

            Apple apple = Instantiate(applePrefab, position, Quaternion.identity);
            apple.transform.SetParent(target, true);
        }
    }

    // Random angle (degrees) at least minSeparation away from everything placed so far.
    // If the circle is too crowded, falls back to the angle with the most free space around it.
    private float PickFreeAngle(System.Random random)
    {
        float best = 0f;
        float bestGap = -1f;

        for (int i = 0; i < placementAttempts; i++)
        {
            float candidate = (float)(random.NextDouble() * 360.0);
            float gap = ClosestGap(candidate);

            if (gap >= minSeparation)
            {
                best = candidate;
                break;
            }
            if (gap > bestGap)
            {
                bestGap = gap;
                best = candidate;
            }
        }

        usedAngles.Add(best);
        return best;
    }

    private float ClosestGap(float angle)
    {
        float closest = 360f;
        foreach (float used in usedAngles)
            closest = Mathf.Min(closest, Mathf.Abs(Mathf.DeltaAngle(angle, used)));
        return closest;
    }
}
