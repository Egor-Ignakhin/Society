using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public static class InventorySpriteContainer
{
    private static readonly Dictionary<string, Sprite> sprites =
        new Dictionary<string, Sprite>();
    static InventorySpriteContainer()
    {
        sprites.Add(NameSprites.Axe, Resources.Load<Sprite>("InventoryItems/Test_Item_Axe"));
        sprites.Add(NameSprites.Pistol1, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol1"));
        sprites.Add(NameSprites.Pistol2, Resources.Load<Sprite>("InventoryItems/Test_Item_Pistol2"));
    }
    public static Sprite GetSprite(string type)
    {
        Debug.LogWarning(type);
        return sprites[type];
    }
    public class NameSprites
    {
        public const string Axe = "Axe";
        public const string Pistol1 = "Pistol1";
        public const string Pistol2 = "Pistol2";
    }
}
