using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DescriptionDrawer : Singleton <DescriptionDrawer>
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject background;
    public TextMeshProUGUI GetText()
    {        
        return text;
    }
    public void SetHint(string str)
    {
        text.SetText(str);
        background.SetActive(!string.IsNullOrEmpty(str));
    }
}
