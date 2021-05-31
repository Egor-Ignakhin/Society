using TMPro;
using UnityEngine;
/// <summary>
/// класс отрисовывающий описание объектов
/// </summary>
public sealed class DescriptionDrawer : Singleton<DescriptionDrawer>
{
    [SerializeField] private TextMeshProUGUI textDesc;
    [SerializeField] private TextMeshProUGUI textTakeKey;
    public void SetHint(string str, string mainType, int count)
    {
        string countStr = count > 1 ? $" x{count}" : string.Empty;
        gameObject.SetActive(!string.IsNullOrEmpty(str));
        if (!gameObject.activeSelf)
            return;
        textDesc.SetText(str + countStr);
        textTakeKey.SetText(Localization.GetUpKeyDescription(mainType, PlayerClasses.PlayerInteractive.InputInteractive));
    }
}
