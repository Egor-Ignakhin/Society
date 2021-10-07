using System;

using TMPro;

using UnityEngine;
namespace Society.SMG
{
    /// <summary>
    /// экая подсказка для помощи игроку. Снять снарягу или надеть.
    /// </summary>
    public class DynamicalElementsAnswer : MonoBehaviour
    {
        private enum TypesPosition { local, global }
        [SerializeField] private GameObject background;
        [SerializeField] private TextMeshProUGUI onOkeybtnText;
        [SerializeField] private TextMeshProUGUI onCancelbtnText;
        [SerializeField] private TypesPosition typesPosition = TypesPosition.global;
        public bool IsActive => background.activeInHierarchy;

        private Action currentOnOkeymtd;
        private Action currentOnCancelmtd;

        private void Awake()
        {
            Hide();
        }
        public void Show(Action onOkeyMethod, Action onCancelMethod, string okeyText = "Да", string cancelText = "Отмена")
        {
            if (typesPosition == TypesPosition.global)
                transform.position = Input.mousePosition;
            else
                transform.localPosition = Input.mousePosition;
            background.SetActive(true);

            currentOnOkeymtd = onOkeyMethod;
            currentOnCancelmtd = onCancelMethod;
            onOkeybtnText.SetText(okeyText);
            onCancelbtnText.SetText(cancelText);
        }

        public void OnOkey()
        {
            Hide();
            currentOnOkeymtd.Invoke();
        }

        public void OnCancel()
        {
            Hide();
            currentOnCancelmtd?.Invoke();
        }
        private void Hide()
        {
            background.SetActive(false);
        }
    }
}