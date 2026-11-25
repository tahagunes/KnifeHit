using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private GameManager gameManager;

    [Header("Sesler")]
    [SerializeField] private AudioClip throwClip;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip failClip;
    [SerializeField] private AudioClip winClip;


    public bool IsMuted => source.mute;

    void Awake()
    {
        source.mute = PlayerPrefs.GetInt(SaveKeys.Muted, 0) == 1;
    }

    void OnEnable()
    {
        Knife.Thrown += OnThrown;
        Knife.Stuck += OnStuck;
        Knife.Failed += OnFailed;
        gameManager.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        Knife.Thrown -= OnThrown;
        Knife.Stuck -= OnStuck;
        Knife.Failed -= OnFailed;
        gameManager.StateChanged -= OnStateChanged;
    }

    private void OnThrown() => Play(throwClip);
    private void OnStuck(Vector2 hitPoint) => Play(hitClip);
    private void OnFailed() => Play(failClip);

    private void OnStateChanged(GameState state)
    {
        if (state == GameState.Won) Play(winClip, false);
    }

    private void Play(AudioClip clip, bool randomizePitch = true)
    {
        source.pitch = randomizePitch ? Random.Range(0.9f, 1.1f) : 1f;
        source.PlayOneShot(clip);
    }

    public void ToggleMute()
    {
        source.mute = !source.mute;
        PlayerPrefs.SetInt(SaveKeys.Muted, source.mute ? 1 : 0);
        PlayerPrefs.Save();
    }
}