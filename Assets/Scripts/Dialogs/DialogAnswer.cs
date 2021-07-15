using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dialogs
{
    public class DialogAnswer : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TextMeshProUGUI text;
        private event Action CalledFunc;

        private void Start()
        {
            Clear();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            CalledFunc?.Invoke();
        }

        public void SetAnswer((string data, Action func) answer)
        {
            text.SetText(answer.data);
            CalledFunc += answer.func;
        }
        public void Clear()
        {
            text.SetText(string.Empty);
            CalledFunc = null;
        }
    }
}