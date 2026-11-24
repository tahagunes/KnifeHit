using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public enum GameState
{
    Playing,
    Won,
    Lost
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private int knivesPerLevel = 7;

    public GameState State { get; private set; } = GameState.Playing;
    public int KnivesLeft { get; private set; }
    public event Action<int> KnivesLeftChanged;
    public event Action<GameState> StateChanged;

    public bool CanThrow => State == GameState.Playing && KnivesLeft > 0;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetState(GameState newState)
{
    State = newState;
    StateChanged?.Invoke(State);
}

    void Awake()
    {
        Time.timeScale = 1f;
        KnivesLeft = knivesPerLevel;
    }

    void OnEnable()
    {
        Knife.Stuck += HandleKnifeStuck;
        Knife.Failed += HandleKnifeFailed;
    }

    void OnDisable()
    {
        Knife.Stuck -= HandleKnifeStuck;
        Knife.Failed -= HandleKnifeFailed;
    }

    public void RegisterThrow()
    {
        KnivesLeft--;
        KnivesLeftChanged?.Invoke(KnivesLeft);
    }

    private void HandleKnifeStuck()
    {
        if (State != GameState.Playing) return;

        if (KnivesLeft == 0)
        {
            SetState(GameState.Won);
            Debug.Log("Kazandın!");
        }
    }

    private void HandleKnifeFailed()
    {
        if (State != GameState.Playing) return;

        SetState(GameState.Lost);
        Debug.Log("Kaybettin!");
    }
}