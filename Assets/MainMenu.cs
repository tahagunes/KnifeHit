using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI playButtonText;

    [Header("Ses")]
    [SerializeField] private Image muteIcon;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;

    void Start()
    {
        int level = PlayerPrefs.GetInt(SaveKeys.Level, 0);
        int coins = Mathf.Min(PlayerPrefs.GetInt(SaveKeys.Coins, 0), Wallet.MaxCoins);

        coinText.text = coins.ToString();
        playButtonText.text = "Oyna - Seviye " + (level + 1);
        UpdateMuteIcon();
    }

    public void Play()
    {
        SceneLoader.Instance.Load("Game");
    }

    // There is no SoundManager in the menu; SoundManager reads this value when the game scene loads.
    public void ToggleMute()
    {
        bool muted = PlayerPrefs.GetInt(SaveKeys.Muted, 0) == 1;
        PlayerPrefs.SetInt(SaveKeys.Muted, muted ? 0 : 1);
        PlayerPrefs.Save();
        UpdateMuteIcon();
    }

    private void UpdateMuteIcon()
    {
        bool muted = PlayerPrefs.GetInt(SaveKeys.Muted, 0) == 1;
        muteIcon.sprite = muted ? soundOffSprite : soundOnSprite;
    }
}
