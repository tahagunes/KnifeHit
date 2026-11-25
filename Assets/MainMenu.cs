using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI playButtonText;

    void Start()
    {
        int level = PlayerPrefs.GetInt(SaveKeys.Level, 0);
        int coins = PlayerPrefs.GetInt(SaveKeys.Coins, 0);

        coinText.text = coins.ToString();
        playButtonText.text = "Oyna - Seviye " + (level + 1);
    }

    public void Play()
    {
        SceneLoader.Instance.Load("Game");
    }
}