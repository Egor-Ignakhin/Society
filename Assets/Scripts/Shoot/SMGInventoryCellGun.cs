using static SMG.ModifierCharacteristics;
[System.Serializable]
public class SMGInventoryCellGun
{
    public int Dispenser = 1;
    public int Silencer = -1;
    public int Title = 0;

    public int AmmoCount = 0;
    internal void SetMag(ModifierIndex index)
    {
        Dispenser = (int)index;
    }
    public void SetTitle(GunTitles t)
    {
        Title = (int)t;
    }
    public void Reload(int title, int dispenser, int silencer, int ammocount)
    {
        Title = title;
        Dispenser = dispenser;
        Silencer = silencer;
        AmmoCount = ammocount;
    }
    internal void Reload(SMGInventoryCellGun GunAk_74)
    {
        if (GunAk_74 is null)
        {
            Clear();
            return;
        }
        Title = GunAk_74.Title;
        Dispenser = GunAk_74.Dispenser;
        Silencer = GunAk_74.Silencer;
        AmmoCount = GunAk_74.AmmoCount;
    }
    public void Clear()
    {
        Dispenser = 0;
        Silencer = 0;
        AmmoCount = 0;
    }
    public void SetAmmoCount(int ac)
    {
        AmmoCount = ac;
    }
}
