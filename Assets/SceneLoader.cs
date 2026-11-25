using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 0.3f;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        fadeGroup.alpha = 0f;
        fadeGroup.blocksRaycasts = false;
    }

    public void Load(string sceneName)
    {
        fadeGroup.blocksRaycasts = true;

        fadeGroup.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneName);

            fadeGroup.DOFade(0f, fadeDuration).SetUpdate(true).OnComplete(() =>
            {
                fadeGroup.blocksRaycasts = false;
            });
        });
    }
}