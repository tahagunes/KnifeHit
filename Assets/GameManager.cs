using UnityEngine;

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

    public bool CanThrow => State == GameState.Playing && KnivesLeft > 0;

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
    }

    private void HandleKnifeStuck()
    {
        if (State != GameState.Playing) return;

        if (KnivesLeft == 0)
        {
            State = GameState.Won;
            Debug.Log("Kazandın!");
        }
    }

    private void HandleKnifeFailed()
    {
        if (State != GameState.Playing) return;

        State = GameState.Lost;
        Debug.Log("Kaybettin!");
    }
}