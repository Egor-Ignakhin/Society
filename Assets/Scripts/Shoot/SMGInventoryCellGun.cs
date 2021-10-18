using static Society.SMG.ModifierCharacteristics;
namespace Society.Inventory
{
    [System.Serializable]
    public class SMGInventoryCellGun
    {
        public int Mag = 1;
        public int Aim = 0;
        public int Silencer = 0;
        public int Title = 0;

        public int AmmoCount = 0;

        public string AmmoType;
        internal void SetMag(ModifierIndex index) => Mag = (int)index;

        public void SetAim(ModifierIndex index) => Aim = (int)index;

        public void SetSilencer(ModifierIndex index) => Silencer = (int)index;


        public void SetTitle(GunTitles t) => Title = (int)t;

        public SMGInventoryCellGun()
        {
            Clear();
        }
        public void Reload(int title, int dispenser, int silencer, int ammoCount, int aim, string ammoType)
        {
            Title = title;
            Mag = dispenser;
            Aim = aim;
            Silencer = silencer;
            AmmoCount = ammoCount;
            AmmoType = ammoType;
        }
        internal void Reload(SMGInventoryCellGun sendedGun)
        {
            if (sendedGun == null)
            {
                Clear();
                return;
            }
            Title = sendedGun.Title;
            Mag = sendedGun.Mag;
            Aim = sendedGun.Aim;
            Silencer = sendedGun.Silencer;
            AmmoCount = sendedGun.AmmoCount;
            AmmoType = sendedGun.AmmoType;
        }
        public void Clear()
        {
            Mag = 0;
            Aim = 0;
            Silencer = 0;
            AmmoCount = 0;
        }
        public void SetAmmoCount(int ac) => AmmoCount = ac;

        public void SetAmmoType(string arg) => AmmoType = arg;
    }
}