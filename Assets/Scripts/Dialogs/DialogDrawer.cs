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