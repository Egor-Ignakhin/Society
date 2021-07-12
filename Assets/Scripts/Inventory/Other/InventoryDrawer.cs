using UnityEngine;

namespace Inventory
{
    public sealed class InventoryDrawer : MonoBehaviour
    {
        [SerializeField] private GameObject mainField;

        [Space(15)]
        [SerializeField] private Transform mainContainer;
        [SerializeField] private Transform supportContainer;

        private bool MainFieldEnabled = false;
        public Transform GetMainContainer() => mainContainer;

        public Transform GetSupportContainer() => supportContainer;
        private EffectsManager effectsManager;


        private void Awake()
        {
            effectsManager = FindObjectOfType<EffectsManager>();
            effectsManager.Init();
        }


        /// <summary>
        /// смена активности инвентаря
        /// </summary>
        public bool ChangeActiveMainField(bool value)
        {
            MainFieldEnabled = !ScreensManager.HasActiveScreen() && value;
            mainField.SetActive(MainFieldEnabled);
            effectsManager.SetEnableSimpleDOF(MainFieldEnabled);
            return MainFieldEnabled;
        }
    }
}