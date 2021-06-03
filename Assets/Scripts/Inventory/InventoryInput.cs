using UnityEngine;
namespace Inventory
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

        public event InputHandler DropEvent;
        public event EventHandler ScrollEvent;

        private const KeyCode changeActiveKeyCode = KeyCode.E;
        private const KeyCode dropCode = KeyCode.BackQuote;
        private bool isEnabled;
        private FirstPersonController fps;
        private Shoots.GunAnimator gunAnimator;
        private void Awake()
        {
            fps = FindObjectOfType<FirstPersonController>();
            gunAnimator = FindObjectOfType<Shoots.GunAnimator>();
        }
        private void Start()
        {
            SetEnable(false);
        }
        private void Update()
        {
            if (ScreensManager.HasActiveScreen() && !isEnabled || gunAnimator.IsAiming)
                return;
            if (Input.GetKeyDown(changeActiveKeyCode))
                SetEnable(isEnabled = !isEnabled);
            if (Input.anyKeyDown)
                SelectCell(Input.inputString);
            if (Input.GetKeyDown(dropCode))
                DropEvent?.Invoke(0);


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

        internal void DropItem(InventoryItem inventoryItem, int count, SMGInventoryCellGun gun)
        {
            var item = Instantiate(inventoryItem, fps.transform.position, fps.transform.rotation);
            item.SetCount(count);
            item.SetGun(gun);
            var powerForce = 5;
            item.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * powerForce, ForceMode.Impulse);
        }

        public void Hide(){
            print(1); SetEnable(false); }
    }
}