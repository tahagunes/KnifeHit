using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITheme : MonoBehaviour
{
    public static readonly Color Primary = Hex("#FF9F43");
    public static readonly Color Secondary = Hex("#2EC4B6");
    public static readonly Color Gold = Hex("#FFD166");
    public static readonly Color Warning = Hex("#EF476F");
    public static readonly Color Text = Color.white;
    public static readonly Color Panel = new Color(0x1B / 255f, 0x1F / 255f, 0x3B / 255f, 0.85f);

    [SerializeField] private Button[] primaryButtons;
    [SerializeField] private Button[] secondaryButtons;
    [SerializeField] private TMP_Text[] goldTexts;
    [SerializeField] private TMP_Text[] warningTexts;
    [SerializeField] private Image[] panels;

    void Awake()
    {
        foreach (TMP_Text text in GetComponentsInChildren<TMP_Text>(true))
            text.color = Text;

        foreach (Button button in primaryButtons) button.image.color = Primary;
        foreach (Button button in secondaryButtons) button.image.color = Secondary;
        foreach (TMP_Text text in goldTexts) text.color = Gold;
        foreach (TMP_Text text in warningTexts) text.color = Warning;
        foreach (Image panel in panels) panel.color = Panel;
    }

    private static Color Hex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
