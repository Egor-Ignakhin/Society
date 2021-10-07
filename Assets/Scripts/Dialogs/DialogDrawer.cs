using System;
using System.Collections.Generic;

using TMPro;

using UnityEngine;
namespace Society.Dialogs
{
    /// <summary>
    /// класс отвечающий за отрисовку диалогов
    /// </summary>
    public sealed class DialogDrawer : MonoBehaviour
    {
        private float delayToDimming;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private UnityEngine.UI.Image backgroundImage;

        [SerializeField] private List<GameObject> allDialogComponents = new List<GameObject>();

        [SerializeField] private TextMeshProUGUI nameAndLevelText;
        [SerializeField] private TextMeshProUGUI fractionText;
        [SerializeField] private TextMeshProUGUI relationText;

        [SerializeField] private Transform dialogsWindowParent;

        private DialogWindow DialogWindowInstance;
        [SerializeField] private List<DialogAnswer> allAnswers = new List<DialogAnswer>();
        private void Start()
        {
            SetEnableAll(false);
            DialogWindowInstance = Resources.Load<DialogWindow>("Dialog_window");
        }
        public void DrawNewDialog(Dialog d, float delay = 1)
        {
            delayToDimming = delay;
            text.SetText(d.Content);
            SetActive(true);
        }

        internal void SetFraction(string fraction)
        {
            fractionText.SetText($"— Фракция {fraction}");
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

        internal void ClearDialogs()
        {
            for (int i = 0; i < dialogsWindowParent.childCount; i++)
            {
                var g = dialogsWindowParent.GetChild(i).gameObject;
                g.transform.SetParent(null);
                Destroy(g);
                i--;
            }
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

        internal void DrawPersonDialog(string pName, string text)
        {
            var dialogWindow = Instantiate(DialogWindowInstance, dialogsWindowParent);
            dialogWindow.OnInit(pName, text);
        }

        internal void SetAnswers((string data, Action func) a1, (string data, Action func) a2, (string data, Action func) a3)
        {
            allAnswers[0].SetAnswer(a1);
            allAnswers[1].SetAnswer(a2);
            allAnswers[2].SetAnswer(a3);
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