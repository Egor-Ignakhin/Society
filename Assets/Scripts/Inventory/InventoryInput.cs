using UnityEngine;
namespace Inventory
{
    /// <summary>
    /// класс отвечающий за считывание ввода пользователя и передачи её управляющему классу
    /// </summary>
    public sealed class InventoryInput : MonoBehaviour
    {
        public delegate void EventHandler(bool value);
        public event EventHandler ChangeActiveEvent;
        public event EventHandler FastMoveCellEvent;

        public delegate void InputHandler(int s);
        public event InputHandler InputKeyEvent;

        public event InputHandler DropEvent;
        public event EventHandler ScrollEvent;

        private const KeyCode changeActiveKeyCode = KeyCode.E;
        private const KeyCode dropCode = KeyCode.Q;
        private bool isEnabled;
        private FirstPersonController fps;
        private void Awake()
        {
            fps = FindObjectOfType<FirstPersonController>();
        }
        private void Update()
        {
            if (InputManager.IsLockeds != 0 && !isEnabled)
                return;
            if (Input.GetKeyDown(changeActiveKeyCode))
            {
                ChangeActive(isEnabled = !isEnabled);
            }
            if (Input.anyKeyDown)
            {
                SelectCell(Input.inputString);
            }
            if (Input.GetKeyDown(dropCode))
            {
                DropEvent?.Invoke(0);
            }

            FastMoveCellEvent?.Invoke(Input.GetKey(KeyCode.LeftShift));

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                SpinCells(scroll > 0);
        }
        /// <summary>
        /// включение видимости инвентаря
        /// </summary>
        private void ChangeActive(bool value) { ChangeActiveEvent?.Invoke(value); }
        private void SpinCells(bool v) { ScrollEvent?.Invoke(v); }
        /// <summary>
        /// насильное выключение инвентаря
        /// </summary>
        public void DisableInventory()
        {
            ChangeActive(false);
            isEnabled = false;
        }
        public void EnableInventory()
        {
            ChangeActive(true);
            isEnabled = true;
        }

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

        internal void DropItem(InventoryItem inventoryItem, int count, SMGGunAk_74 gun)
        {
            var item = Instantiate(inventoryItem, fps.transform.position, fps.transform.rotation);
            item.SetCount(count);
            item.SetGun(gun);
            var powerForce = 2;
            item.GetComponent<Rigidbody>().AddForce(fps.transform.forward * powerForce, ForceMode.Impulse);
        }
    }
}