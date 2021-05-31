using PlayerClasses;

namespace SMG
{
    /// <summary>
    /// класс отвечающий за верстак
    /// </summary>
    class WorkbenchSMG : InteractiveObject
    {
        private SMGMain main;
        private void Start()
        {
            main = FindObjectOfType<SMGMain>();
            SetType("Workbench");
        }
        public override void Interact(PlayerStatements pl)
        {
            if (!ScreensManager.HasActiveScreen())
                main.SetEnable(true);
        }
    }
}