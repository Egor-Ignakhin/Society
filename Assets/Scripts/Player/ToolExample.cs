using Society.Inventory;

using UnityEngine;

public class ToolExample : MonoBehaviour
{
    [SerializeField] private ItemStates.ItemsID id;

    internal int GetID()
    {
        return (int)id;
    }
}
