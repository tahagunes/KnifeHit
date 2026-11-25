using UnityEngine;
using UnityEngine.UI;

public class StageProgressUI : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Image[] dots;
    [SerializeField] private Color doneColor = new Color(1f, 0.62f, 0.26f);
    [SerializeField] private Color currentColor = Color.white;
    [SerializeField] private Color futureColor = new Color(1f, 1f, 1f, 0.25f);
    [SerializeField] private Color bossColor = new Color(0.94f, 0.28f, 0.44f);

    void Start()
    {
        int cyclePos = gameManager.CurrentLevel % dots.Length;

        for (int i = 0; i < dots.Length; i++)
        {
            bool isBossDot = i == dots.Length - 1;

            if (i < cyclePos) dots[i].color = doneColor;
            else if (i == cyclePos) dots[i].color = isBossDot ? bossColor : currentColor;
            else dots[i].color = isBossDot ? new Color(bossColor.r, bossColor.g, bossColor.b, 0.4f) : futureColor;

            dots[i].transform.localScale = i == cyclePos ? Vector3.one * 1.3f : Vector3.one;
        }
    }
}
