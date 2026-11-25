using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public const int MaxCoins = 999;

    private int savedCoins;
    // Coins from apples collected in this level; only saved if the level is won.
    private int pendingCoins;

    public int Coins => Mathf.Min(savedCoins + pendingCoins, MaxCoins);
    public event Action<int> CoinsChanged;

    void Awake()
    {
        savedCoins = Mathf.Clamp(PlayerPrefs.GetInt(SaveKeys.Coins, 0), 0, MaxCoins);
    }

    void OnEnable()
    {
        Apple.Collected += AddPending;
        gameManager.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        Apple.Collected -= AddPending;
        gameManager.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Won)
        {
            int reward = pendingCoins;
            if (gameManager.CurrentLevelData.isBoss)
                reward += gameManager.CurrentLevelData.bossReward;

            pendingCoins = 0;
            Add(reward);
        }
        else if (state == GameState.Lost)
        {
            pendingCoins = 0;
            CoinsChanged?.Invoke(Coins);
        }
    }

    private void AddPending(int amount)
    {
        if (gameManager.State == GameState.Lost) return;

        // The final knife can collect an apple after the level is already won.
        if (gameManager.State == GameState.Won)
        {
            Add(amount);
            return;
        }

        pendingCoins += amount;
        CoinsChanged?.Invoke(Coins);
    }

    public void Add(int amount)
    {
        savedCoins = Mathf.Clamp(savedCoins + amount, 0, MaxCoins);
        PlayerPrefs.SetInt(SaveKeys.Coins, savedCoins);
        PlayerPrefs.Save();
        CoinsChanged?.Invoke(Coins);
    }
}
