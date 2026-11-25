using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Wallet wallet;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI bossText;

    [Header("Seviye ilerlemesi")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI bossInfoText;

    [Header("Paneller")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI earnedText;
    [SerializeField] private TextMeshProUGUI loseLevelText;

    private int coinsAtStart;

    void OnEnable()
    {
        gameManager.StateChanged += HandleStateChanged;
        wallet.CoinsChanged += UpdateCoins;
    }

    void OnDisable()
    {
        gameManager.StateChanged -= HandleStateChanged;
        wallet.CoinsChanged -= UpdateCoins;
    }

    void Start()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        UpdateCoins(wallet.Coins);
        coinsAtStart = wallet.Coins;

        UpdateLevelTexts();
        PlayBossIntro();
    }

    // The dots themselves are handled by StageProgressUI.
    private void UpdateLevelTexts()
    {
        int cycleLength = gameManager.CycleLength;
        int position = gameManager.CyclePosition;
        bool isBoss = gameManager.CurrentLevelData.isBoss;

        levelText.text = "Seviye " + (gameManager.CurrentLevel + 1);

        int levelsToBoss = cycleLength - 1 - position;
        bossInfoText.color = isBoss ? UITheme.Warning : Color.white;
        bossInfoText.text = isBoss ? "BOSS SEVİYESİ" : "Boss'a " + levelsToBoss + " seviye";
    }

    private void PlayBossIntro()
    {
        bool isBoss = gameManager.CurrentLevelData.isBoss;
        bossText.gameObject.SetActive(isBoss);
        if (!isBoss) return;

        bossText.transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence().SetLink(bossText.gameObject);
        sequence.Append(bossText.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
        sequence.AppendInterval(1f);
        sequence.Append(bossText.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack));
    }

    private void HandleStateChanged(GameState state)
    {
        if (state == GameState.Won)
        {
            bossText.gameObject.SetActive(false);
            ShowPanel(winPanel);
        }
        else if (state == GameState.Lost)
        {
            bossText.gameObject.SetActive(false);
            loseLevelText.text = "Seviye " + (gameManager.CurrentLevel + 1);
            ShowPanel(losePanel);
        }
    }

    // Wallet also listens to StateChanged and the listener order is not guaranteed, so the earned
    // amount is read in OnStart, after the delay, when every listener (incl. the boss reward) has run.
    private void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        panel.transform.DOScale(1f, 0.35f)
            .SetEase(Ease.OutBack)
            .SetDelay(0.6f)
            .OnStart(() => earnedText.text = "+" + (wallet.Coins - coinsAtStart))
            .SetLink(panel);
    }

    private void UpdateCoins(int coins)
    {
        coinText.text = coins.ToString();
    }
}
