using UnityEngine;
using static SMG.SMGModifierCharacteristics;
[System.Serializable]
public class SMGGunAk_74
{
    public int Dispenser = 1;
    public int Silencer = -1;

    internal void SetMag(ModifierIndex index)
    {        
        Debug.Log(index);
        Dispenser = (int)index;
    }

    internal void Reload(SMGGunAk_74 GunAk_74)
    {
        Dispenser = GunAk_74.Dispenser;
        Silencer = GunAk_74.Silencer;
    }
}
