using TMPro;

using UnityEngine;

namespace Society.Dialogs
{
    public sealed class DialogWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        public void OnInit(string pName, string mText)
        {
            mainText.SetText($"{pName}\n{mText}");
        }
    }
}