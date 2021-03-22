using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Dialogs
{
    public sealed class DialogDrawer : MonoBehaviour
    {
        private float delayToDimming;
        [SerializeField] private TMPro.TextMeshProUGUI text;
        public void DrawNewDialog(Dialog d, float delay = 1)
        {
            delayToDimming = delay;
            text.SetText(d.Content);
            EnableDialog();
        }
        private void Update()
        {
            if(delayToDimming > 0)
            {
                delayToDimming -= Time.deltaTime;
            }
            else
            {
                DisableDialog();
            }
        }

        private void EnableDialog()
        {
            text.enabled = true;
        }
        private void DisableDialog()
        {
            text.enabled = false;
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