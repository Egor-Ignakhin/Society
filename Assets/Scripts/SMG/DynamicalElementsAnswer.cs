using System;
using TMPro;
using UnityEngine;
namespace SMG
{
    /// <summary>
    /// экая подсказка для помощи игроку. Снять снарягу или надеть.
    /// </summary>
    public class DynamicalElementsAnswer : MonoBehaviour
    {
        [SerializeField] private GameObject background;        
        [SerializeField] private TextMeshProUGUI onOkeybtnText;
        [SerializeField] private TextMeshProUGUI onCancelbtnText;
        public bool IsActive => background.activeInHierarchy;

        private Action currentOnOkeymtd;
        private Action currentOnCancelmtd;
        private void Awake()
        {
            Hide();
        }
        public void Show(Action onOkeyMethod, Action onCancelMethod, string okeyText = "Да", string cancelText = "Отмена")
        {
            transform.position = Input.mousePosition;
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