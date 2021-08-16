using TMPro;
using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс отрисовывающий описание объектов
    /// </summary>
    public sealed class DescriptionDrawer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDesc;
        [SerializeField] private TextMeshProUGUI textTakeKey;
        private bool canChangeHint = true;
        private void Awake()
        {
            FindObjectOfType<Missions.MissionsManager>().SetDescriptionDrawer(this);
        }
        public void SetHint(string str, string mainType, int count)
        {
            if (!canChangeHint)
                return;

            string countStr = count > 1 ? $" x{count}" : string.Empty;
            gameObject.SetActive(!string.IsNullOrEmpty(str));
            if (!gameObject.activeSelf)
                return;
            textDesc.SetText(str + countStr);
            textTakeKey.SetText(Localization.GetUpKeyDescription(mainType, PlayerClasses.PlayerInteractive.InputInteractive));
        }

        internal void SetIrremovableHint(string v)
        {
            textTakeKey.SetText(v);            
            canChangeHint = string.IsNullOrEmpty(v);
            textDesc.SetText(string.Empty);
            gameObject.SetActive(!canChangeHint);
        }
    }
}