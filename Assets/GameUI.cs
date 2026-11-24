using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI knifeCountText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI levelText;

    void OnEnable()
    {
        gameManager.KnivesLeftChanged += UpdateKnifeCount;
        gameManager.StateChanged += HandleStateChanged;
    }

    void OnDisable()
    {
        gameManager.KnivesLeftChanged -= UpdateKnifeCount;
        gameManager.StateChanged -= HandleStateChanged;
    }

    void Start()
    {
        UpdateKnifeCount(gameManager.KnivesLeft);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        levelText.text = "Seviye " + (gameManager.CurrentLevel + 1);
    }

    private void UpdateKnifeCount(int count)
    {
        knifeCountText.text = "Bıçak: " + count;
    }

    private void HandleStateChanged(GameState state)
    {
        winPanel.SetActive(state == GameState.Won);
        losePanel.SetActive(state == GameState.Lost);
    }
}