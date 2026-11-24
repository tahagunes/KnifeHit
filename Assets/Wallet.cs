using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private const string CoinsKey = "Coins";

    public int Coins { get; private set; }
    public event Action<int> CoinsChanged;

    void Awake()
    {
        Coins = PlayerPrefs.GetInt(CoinsKey, 0);
    }

    void OnEnable()
    {
        Apple.Collected += Add;
        gameManager.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        Apple.Collected -= Add;
        gameManager.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Won && gameManager.CurrentLevelData.isBoss)
        {
            Add(gameManager.CurrentLevelData.bossReward);
        }
    }

    public void Add(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
        CoinsChanged?.Invoke(Coins);
    }
}