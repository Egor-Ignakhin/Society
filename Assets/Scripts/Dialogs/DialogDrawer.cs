using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace Dialogs
{/// <summary>
/// класс отвечающий за отрисовку диалогов
/// </summary>
    public sealed class DialogDrawer : MonoBehaviour
    {
        private float delayToDimming;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UnityEngine.UI.Image backgroundImage;

        [SerializeField] private List<GameObject> allDialogComponents = new List<GameObject>();

        [SerializeField] private TextMeshProUGUI nameAndLevelText;
        [SerializeField] private TextMeshProUGUI relationText;
        private void Start()
        {
            SetEnableAll(false);
        }
        public void DrawNewDialog(Dialog d, float delay = 1)
        {
            delayToDimming = delay;
            text.SetText(d.Content);
            SetActive(true);
        }
        private void Update()
        {
            if (delayToDimming > 0)
            {
                delayToDimming -= Time.deltaTime;
            }
            else
            {
                SetActive(false);
            }
        }

        private void SetActive(bool v)
        {
            backgroundImage.enabled = text.enabled = v;
        }

        internal void SetRelationAtPlayer(string prp)
        {
            relationText.SetText($"Отношение    {prp}");
        }

        internal void SetEnableAll(bool v)
        {
            foreach (var g in allDialogComponents)
            {
                g.SetActive(v);
            }
        }
        public void SetNameAndLevel(string pName, int lvl)
        {
            nameAndLevelText.SetText($"{pName} | {lvl} уровень");
        }
    }
    public class Dialog
    {
        public readonly string Content;
        public Dialog(string c)
        {
            Content = c;
        }
    }
}