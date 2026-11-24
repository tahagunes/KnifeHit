using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI knifeCountText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Wallet wallet;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI bossText;
    [SerializeField] private TextMeshProUGUI rewardText;

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Won)
        {
            LevelData level = gameManager.CurrentLevelData;
            rewardText.gameObject.SetActive(level.isBoss);
            rewardText.text = "+" + level.bossReward + " Coin";
            ShowPanel(winPanel);
        }
        if (state == GameState.Lost) ShowPanel(losePanel);
    }

    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
        panel.transform.DOScale(1f, 0.35f).SetEase(Ease.OutBack).SetDelay(0.6f);
    }

    void OnEnable()
    {
        gameManager.KnivesLeftChanged += UpdateKnifeCount;
        gameManager.StateChanged += HandleStateChanged;
        wallet.CoinsChanged += UpdateCoins;
    }

    void OnDisable()
    {
        gameManager.KnivesLeftChanged -= UpdateKnifeCount;
        gameManager.StateChanged -= HandleStateChanged;
        wallet.CoinsChanged -= UpdateCoins;
    }

    void Start()
    {
        UpdateKnifeCount(gameManager.KnivesLeft);
        UpdateCoins(wallet.Coins);
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        levelText.text = "Seviye " + (gameManager.CurrentLevel + 1);

        bool isBoss = gameManager.CurrentLevelData.isBoss;
        bossText.gameObject.SetActive(isBoss);

        if (isBoss)
        {
            levelText.text += " - BOSS";

            bossText.transform.localScale = Vector3.zero;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(bossText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            sequence.AppendInterval(1f);
            sequence.Append(bossText.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        }
    }

    private void UpdateKnifeCount(int count)
    {
        knifeCountText.text = "Bıçak: " + count;
    }

    private void UpdateCoins(int coins)
    {
        coinText.text = "Coin: " + coins;
    }
}