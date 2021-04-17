using TMPro;
using UnityEngine;
/// <summary>
/// класс отрисовывающий описание объектов
/// </summary>
public class DescriptionDrawer : Singleton <DescriptionDrawer>
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject background;// фон
    public void SetHint(string str)
    {
        text.SetText(str);
        background.SetActive(!string.IsNullOrEmpty(str));
    }
}
