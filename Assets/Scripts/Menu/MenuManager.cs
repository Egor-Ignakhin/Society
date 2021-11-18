using System;

using UnityEngine;

namespace Society.Menu
{
    public abstract class MenuManager : MonoBehaviour
    {
        [SerializeField] protected GameObject genericPanel;
        internal void ShowGenericPanel()
        {
            genericPanel.SetActive(true);
        }
    }
}