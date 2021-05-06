using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// класс отрисовывающий описание объектов
/// </summary>
public sealed class DescriptionDrawer : Singleton<DescriptionDrawer>
{
    [SerializeField] private TextMeshProUGUI textDesc;
    [SerializeField] private TextMeshProUGUI textTakeKey;
    public void SetHint(string str, string mainType)
    {
        textDesc.SetText(str);
        textTakeKey.SetText(Localization.GetUpKeyDescription(mainType, PlayerClasses.PlayerInteractive.InputInteractive));
        gameObject.SetActive(!string.IsNullOrEmpty(str));

    }    
}
