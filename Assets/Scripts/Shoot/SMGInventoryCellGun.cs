using UnityEngine;
using static SMG.ModifierCharacteristics;
[System.Serializable]
public class SMGInventoryCellGun : Object
{
    public int Mag = 1;
    public int Aim = 0;
    public int Silencer = 0;
    public int Title = 0;

    public int AmmoCount = 0;
    internal void SetMag(ModifierIndex index) => Mag = (int)index;

    public void SetAim(ModifierIndex index) => Aim = (int)index;

    public void SetSilencer(ModifierIndex index) => Silencer = (int)index;


    public void SetTitle(GunTitles t) => Title = (int)t;

    public void Reload(int title, int dispenser, int silencer, int ammocount, int aim)
    {
        Title = title;
        Mag = dispenser;
        Aim = aim;
        Silencer = silencer;
        AmmoCount = ammocount;
    }
    internal void Reload(SMGInventoryCellGun GunAk_74)
    {
        if (GunAk_74 == null)
        {
            Clear();
            return;
        }
        Title = GunAk_74.Title;
        Mag = GunAk_74.Mag;
        Aim = GunAk_74.Aim;
        Silencer = GunAk_74.Silencer;
        AmmoCount = GunAk_74.AmmoCount;
    }
    public void Clear()
    {
        Mag = 0;
        Aim = 0;
        Silencer = 0;
        AmmoCount = 0;
    }
    public void SetAmmoCount(int ac) => AmmoCount = ac;
}
