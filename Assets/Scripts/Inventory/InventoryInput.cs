using Society.GameScreens;
using Society.Localization;
using Society.Player.Controllers;
using Society.SMG;

using System;

using UnityEngine;
namespace Society.Inventory
{
    /// <summary>
    /// класс отвечающий за считывание ввода пользователя и передачи её управляющему классу
    /// </summary>
    public sealed class InventoryInput : MonoBehaviour, IGameScreen
    {
        public delegate void EventHandler(bool value);
        public event EventHandler ChangeActiveEvent;
        public event EventHandler FastMoveCellEvent;

        public delegate void InputHandler(int s);
        public event InputHandler InputKeyEvent;

        public event Action<string, int> DropEvent;
        public event Action InputKeyDrop;
        public event EventHandler ScrollEvent;

        private const KeyCode changeActiveKeyCode = KeyCode.E;
        private const KeyCode dropCode = KeyCode.BackQuote;
        private bool isEnabled;
        private FirstPersonController fps;
        private Shoot.GunAnimator gunAnimator;
        private bool canInteractive = true;
        private void Awake()
        {
            fps = FindObjectOfType<FirstPersonController>();
            gunAnimator = FindObjectOfType<Shoot.GunAnimator>();
        }

        internal void SetInteractive(bool v) => canInteractive = v;

        private void Start() => SetEnable(false);

        private void Update()
        {
            if (!canInteractive)
                return;
            if (ScreensManager.HasActiveScreen() && !isEnabled || gunAnimator.IsAiming)
                return;
            if (Input.GetKeyDown(changeActiveKeyCode))
                SetEnable(isEnabled = !isEnabled);
            if (Input.anyKeyDown)
                SelectCell(Input.inputString);
            if (Input.GetKeyDown(dropCode))
                InputKeyDrop?.Invoke();


            FastMoveCellEvent?.Invoke(Input.GetKey(KeyCode.LeftShift));

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                SpinCells(scroll > 0);
        }
        /// <summary>
        /// включение видимости инвентаря
        /// </summary>
        public void SetEnable(bool value)
        {
            isEnabled = value;
            ChangeActiveEvent?.Invoke(value);
            ScreensManager.SetScreen(isEnabled ? this : null);
        }
        private void SpinCells(bool v) => ScrollEvent?.Invoke(v);

        /// <summary>
        /// выделение слотов при нажатии клавиш
        /// </summary>
        /// <param name="input"></param>
        private void SelectCell(string input)
        {
            if (input == string.Empty)
                return;

            if (int.TryParse(input, out int s))
                InputKeyEvent?.Invoke(s);
        }

        internal void DropItem(InventoryItem inventoryItem, int id, int count, SMGInventoryCellGun gun)
        {
            var item = Instantiate(inventoryItem, fps.transform.position, fps.transform.rotation);
            item.SetCount(count);
            item.SetGun(gun);
            var powerForce = 5;
            item.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * powerForce, ForceMode.Impulse);

            DropEvent?.Invoke(LocalizationManager.GetHint(id), count);
        }

        internal void DropModifier(ModifierCharacteristics.SMGTitleTypeIndex tti)
        {
            var pref = ModifiersPrefabsData.GetPrefabFromTTI(tti);
            var item = Instantiate(pref, fps.transform.position, fps.transform.rotation);
            var powerForce = 5;
            item.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * powerForce, ForceMode.Impulse);


            DropEvent?.Invoke(GunCharacteristics.GetNormalTitleFromTTI(tti), 1);
        }

        public bool Hide()
        {
            SetEnable(false);
            return true;
        }

        public KeyCode HideKey() => KeyCode.Escape;
    }
}