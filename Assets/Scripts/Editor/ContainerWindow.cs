using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ContainerWindow : EditorWindow
{
    private Vector2 scrollPos = new Vector2(10, 10);

    [MenuItem("Window/GUI Containers Data")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ContainerWindow));
    }
    private void OnGUI()
    {
        GUILayout.Label("Containers Data", EditorStyles.boldLabel);
        Object selectedObject = Selection.activeObject;
        if (!(selectedObject as GameObject).TryGetComponent<Inventory.ItemsContainer>(out var container))
            return;

        EditorGUILayout.LabelField(Selection.activeObject.name);        
        EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(250));
        //      GUILayout.BeginHorizontal();
        var data = container.GetStartedData();
        for (int i = 0; i < data.items.Count; i++)
        {
            try
            {
                Texture2D itemTexture = null;
                if (((Inventory.ItemStates.ItemsID)data.items[i]) == Inventory.ItemStates.ItemsID.Default)
                {
                    itemTexture = new Texture2D(2, 2);
                }
                else
                {
                    itemTexture = Resources.Load<Sprite>($"InventoryItems\\{(Inventory.ItemStates.ItemsID)data.items[i]}").texture;
                }
                GUILayout.Label(itemTexture);
            }
            catch
            {
                Debug.Log($"InventoryItems\\{(Inventory.ItemStates.ItemsID)data.items[i]}");
            }
        }        
    }
}
