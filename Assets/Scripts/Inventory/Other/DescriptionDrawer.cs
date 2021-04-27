using TMPro;
using UnityEngine;
/// <summary>
/// класс отрисовывающий описание объектов
/// </summary>
public class DescriptionDrawer : Singleton<DescriptionDrawer>
{
    [SerializeField] private TextMeshProUGUI textDesc;
    [SerializeField] private TextMeshProUGUI textTakeKey;
    public void SetHint(string str)
    {
        textDesc.SetText(str);
        textTakeKey.SetText(DefaultTakeUpKey);
        gameObject.SetActive(!string.IsNullOrEmpty(str));
    }
    private readonly string DefaultTakeUpKey = $"Поднять предмет ({PlayerClasses.PlayerInteractive.InputInteractive})";
}
