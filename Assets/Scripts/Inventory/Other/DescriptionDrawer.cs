using Society.Player;
using Society.Settings;

using TMPro;

using UnityEngine;
namespace Society.Inventory.Other
{
    /// <summary>
    /// класс отрисовывающий описание объектов
    /// </summary>
    public sealed class DescriptionDrawer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textDesc;
        [SerializeField] private TextMeshProUGUI textTakeKey;
        private bool canChangeHint = true;
        private PlayerInteractive playerInteractive;
        private void Awake()
        {
            FindObjectOfType<Missions.MissionsManager>().SetDescriptionDrawer(this);
            playerInteractive = FindObjectOfType<PlayerInteractive>();
        }
        private void FixedUpdate()
        {
            if (!canChangeHint)
                return;

            (string str, string mainType, int count) = (playerInteractive.
                GetiISubDescription().ToString(),
                playerInteractive.GetiIMainDescription().ToString(),
                playerInteractive.GetIICount());

            string countStr = count > 1 ? $" x{count}" : string.Empty;

            bool drawerIsActive = !string.IsNullOrEmpty(str);

            textDesc.enabled = drawerIsActive;
            textTakeKey.enabled = drawerIsActive;

            if (!drawerIsActive)
                return;

            textDesc.SetText(str + countStr);
            textTakeKey.SetText(Localization.LocalizationManager.GetUpKeyDescription(mainType, GameSettings.GetInteractionKeyCode()));
        }

        /// <summary>
        /// Установить неудаляемую подсказку
        /// </summary>
        /// <param name="v"></param>
        internal void SetIrremovableHint(string v)
        {
            textTakeKey.SetText(v);
            canChangeHint = string.IsNullOrEmpty(v);
            textDesc.SetText(string.Empty);

            textDesc.enabled = !canChangeHint;
            textTakeKey.enabled = !canChangeHint;
        }
    }
}