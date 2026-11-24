using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
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
    }

    void OnDisable()
    {
        Apple.Collected -= Add;
    }

    public void Add(int amount)
    {
        Coins += amount;
        PlayerPrefs.SetInt(CoinsKey, Coins);
        PlayerPrefs.Save();
        CoinsChanged?.Invoke(Coins);
    }
}