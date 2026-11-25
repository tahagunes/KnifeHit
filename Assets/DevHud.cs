using TMPro;
using UnityEngine;

public class DevHud : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TargetRotator targetRotator;

    void Update()
    {
        LevelData level = gameManager.CurrentLevelData;

        text.text =
            $"Seviye: {gameManager.CurrentLevel + 1}\n" +
            $"Hız: {targetRotator.CurrentSpeed:F0}°/s (formül: {level.rotationSpeed:F0})\n" +
            $"Bıçak: {gameManager.KnivesLeft}/{level.knifeCount}\n" +
            $"Elma: {level.appleCount}  Engel: {level.obstacleKnives}\n" +
            $"Boss: {(level.isBoss ? "Evet" : "Hayır")}";
    }
}