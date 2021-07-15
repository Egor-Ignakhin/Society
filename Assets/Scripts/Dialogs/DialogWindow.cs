using TMPro;
using UnityEngine;

namespace Dialogs
{
    public sealed class DialogWindow : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI mainText;
        [SerializeField] private TextMeshProUGUI personName;
        public void OnInit(string pName, string mText)
        {
            personName.SetText(pName);
            mainText.SetText(mText);            
        }
    }
}