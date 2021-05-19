using PlayerClasses;

namespace SMG
{
    /// <summary>
    /// класс отвечающий за верстак
    /// </summary>
    public class WorkbenchSMG : InteractiveObject, IGameScreen
    {
        private SMGMain main;
        private void Start()
        {
            main = FindObjectOfType<SMGMain>();
        }
        public override void Interact(PlayerStatements pl)
        {
            main.SetEnableMaps(true);
        }
    }
}