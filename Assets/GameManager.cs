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
    public GameState State { get; private set; } = GameState.Playing;
    public int KnivesLeft { get; private set; }
    public event Action<int> KnivesLeftChanged;
    public event Action<GameState> StateChanged;
    [SerializeField] private LevelData[] levels;
    [SerializeField] private TargetRotator targetRotator;
    private const string LevelKey = "CurrentLevel";
    public int CurrentLevel { get; private set; }
    public bool CanThrow => State == GameState.Playing && KnivesLeft > 0;
    public LevelData CurrentLevelData { get; private set; }

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

        CurrentLevel = PlayerPrefs.GetInt(LevelKey, 0);
        LevelData level = levels[CurrentLevel % levels.Length];

        KnivesLeft = level.knifeCount;
        targetRotator.SetSpeed(level.rotationSpeed);
        CurrentLevelData = levels[CurrentLevel % levels.Length];
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt(LevelKey, CurrentLevel + 1);
        PlayerPrefs.Save();
        Restart();
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

    private void HandleKnifeStuck(Vector2 hitPoint)
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

[System.Serializable]
public class LevelData
{
    public int knifeCount = 7;
    public float rotationSpeed = 150f;
    public int appleCount = 1;
}