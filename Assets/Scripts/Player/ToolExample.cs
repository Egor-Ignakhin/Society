using System;
using UnityEngine;

public class ToolExample : MonoBehaviour
{
    [SerializeField] private Inventory.ItemStates.ItemsID id;

    internal int GetID()
    {
        return (int)id;
    }
}
