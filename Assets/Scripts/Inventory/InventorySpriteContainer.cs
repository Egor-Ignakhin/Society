using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// класс сод. словарь спрайтов для инвентаря
/// </summary>
public static class InventorySpriteContainer
{
    private static readonly Dictionary<string, Sprite> sprites;
    static InventorySpriteContainer()
    {
        sprites = new Dictionary<string, Sprite> {
            { NameSprites.DefaultIcon, Resources.Load<Sprite>("InventoryItems/Test_Item_DefaultIcon") },
            { NameSprites.Axe, Resources.Load<Sprite>("InventoryItems/Test_Item_Axe") },
            { NameSprites.Pistol1, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol1") },
            { NameSprites.Pistol2, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol2") } };
    }
    public static Sprite GetSprite(string type)
    {
        return sprites[type];
    }
    public class NameSprites
    {
        public const string DefaultIcon = "DefaultIcon";
        public const string Axe = "Axe";
        public const string Pistol1 = "Pistol1";
        public const string Pistol2 = "Pistol2";
    }
}
