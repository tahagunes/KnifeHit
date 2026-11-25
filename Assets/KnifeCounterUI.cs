using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeCounterUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image iconPrefab;
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private Color usedColor = new Color(1f, 1f, 1f, 0.2f);

    private readonly List<Image> icons = new List<Image>();

    void OnEnable() => gameManager.KnivesLeftChanged += Refresh;
    void OnDisable() => gameManager.KnivesLeftChanged -= Refresh;

    void Start()
    {
        int total = gameManager.CurrentLevelData.knifeCount;
        for (int i = 0; i < total; i++)
        {
            icons.Add(Instantiate(iconPrefab, transform));
        }
        Refresh(gameManager.KnivesLeft);
    }

    private void Refresh(int knivesLeft)
    {
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].color = i < knivesLeft ? activeColor : usedColor;
        }
    }
}
