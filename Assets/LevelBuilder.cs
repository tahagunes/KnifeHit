using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform target;
    [SerializeField] private Apple applePrefab;
    [SerializeField] private Knife knifePrefab;

    void Start()
    {
        float radius = target.GetComponent<CircleCollider2D>().radius * target.lossyScale.x;
        int count = gameManager.CurrentLevelData.appleCount;

        for (int i = 0; i < count; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            Vector3 position = target.position + (Vector3)(direction * radius);

            Apple apple = Instantiate(applePrefab, position, Quaternion.identity);
            apple.transform.SetParent(target, true);
        }

        int obstacleCount = gameManager.CurrentLevelData.obstacleKnives;

        for (int i = 0; i < obstacleCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            Knife knife = Instantiate(knifePrefab);
            knife.PlaceOnTarget(target, angle);
        }
    }
}