using UnityEngine;
namespace Dialogs
{
    public sealed class DialogDrawer : Singleton<DialogDrawer>// класс отвечающий за отрисовку диалогов
    {
        private float delayToDimming;
        [SerializeField] private TMPro.TextMeshProUGUI text;
        [SerializeField] private UnityEngine.UI.Image backgroundImage;
        public void DrawNewDialog(Dialog d, float delay = 1)
        {
            delayToDimming = delay;
            text.SetText(d.Content);
            EnableDialog();
        }
        private void Update()
        {
            if (delayToDimming > 0)
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
            backgroundImage.enabled = text.enabled = true;
        }
        private void DisableDialog()
        {
            backgroundImage.enabled = text.enabled = false;
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