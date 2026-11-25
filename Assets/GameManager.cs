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
    [SerializeField] private LevelGenSettings genSettings;
    [SerializeField] private float knifeWidth = 0.2f;
    [SerializeField] private float knifeSpacing = 1.5f;
    [SerializeField] private TargetRotator targetRotator;
    public int CurrentLevel { get; private set; }
    public bool CanThrow => State == GameState.Playing && KnivesLeft > 0;
    public LevelData CurrentLevelData { get; private set; }
    public int CycleLength => genSettings.cycleLength;
    // 0-based position of the current level inside the boss cycle (last position is the boss).
    public int CyclePosition => CurrentLevel % genSettings.cycleLength;

    public void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        LoadScene("MainMenu");
    }

    private void LoadScene(string sceneName)
    {
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.Load(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }

    private void SetState(GameState newState)
{
    State = newState;
    StateChanged?.Invoke(State);
}

    void Awake()
    {
        Time.timeScale = 1f;

        CurrentLevel = PlayerPrefs.GetInt(SaveKeys.Level, 0);
        CurrentLevelData = LevelGenerator.Generate(CurrentLevel + 1, CalculateSlotCount(), genSettings);

        KnivesLeft = CurrentLevelData.knifeCount;
        targetRotator.Configure(CurrentLevelData.rotationSpeed, CurrentLevelData.isBoss);
    }

    private int CalculateSlotCount()
    {
        var circle = targetRotator.GetComponent<CircleCollider2D>();
        float radius = circle.radius * targetRotator.transform.lossyScale.x;
        float circumference = 2f * Mathf.PI * radius;
        return Mathf.FloorToInt(circumference / (knifeWidth * knifeSpacing));
    }

    public void NextLevel()
    {
        SaveProgress();
        Restart();
    }

    // Saved as soon as the level is won, so leaving via the menu button keeps the progress.
    private void SaveProgress()
    {
        PlayerPrefs.SetInt(SaveKeys.Level, CurrentLevel + 1);
        PlayerPrefs.Save();
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
            SaveProgress();
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
    public int knifeCount;
    public float rotationSpeed;
    public int appleCount;
    public int obstacleKnives;
    public bool isBoss;
    public int bossReward;
    public int seed;
}