using System;
using UnityEditor;
using UnityEngine;
using Inventory;
using System.Collections.Generic;

public class ContainerWindow : EditorWindow
{
#if UNITY_EDITOR
    private Vector2 verticalScrollPosition;
    [MenuItem("Window/GUI Containers Data")]
    public static void ShowWindow()
    {
        GetWindow(typeof(ContainerWindow));
    }

    private void OnGUI()
    {
        autoRepaintOnSceneChange = true;
        GUILayout.Label("Containers Data", EditorStyles.boldLabel);
        UnityEngine.Object selectedObject = Selection.activeObject;
        if (!selectedObject)
            return;
        var obj = selectedObject as GameObject;

        if (!obj)
            return;

        if (!obj.TryGetComponent<ItemsContainer>(out var container))
            return;

        EditorGUILayout.LabelField($"Container     {Selection.activeObject.name}");
        (List<int> items, List<int> count) = container.GetStartedData();
        DrawFreeItems(items.Count, container);

        GUILayout.Space(25);

        DrawContentCount(items.Count);

        DrawCurrentContent(items, count, container);
    }

    private void DrawFreeItems(int currentCount, ItemsContainer container)
    {
        EditorGUILayout.LabelField($"All items");
        EditorGUILayout.BeginHorizontal();
        for (int i = 1; i < Enum.GetNames(typeof(ItemStates.ItemsID)).Length - 1; i++)
        {
            var iT = Resources.Load<Sprite>($"InventoryItems\\{(ItemStates.ItemsID)i}").texture;
            if (GUILayout.Button(iT, GUILayout.MaxHeight(position.height / 12.5f), GUILayout.MaxWidth(position.width / 5)) &&
                (currentCount < ItemsContainer.maxCells))
            {
                container.AddStartedItem(i);
            }
            if (i % 5 == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawContentCount(int count)
    {
        if (count < ItemsContainer.maxCells)
        {
            GUI.contentColor = Color.white;
        }
        else
        {
            GUI.contentColor = Color.red;
        }
        EditorGUILayout.LabelField($"Content used [{count} / {ItemsContainer.maxCells}]");
        GUI.contentColor = Color.white;
    }
    private void DrawCurrentContent(List<int> items, List<int> count, ItemsContainer container)
    {
        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < items.Count; i++)
        {
            try
            {
                EditorGUILayout.BeginVertical();
                Texture2D itemTexture = null;
                if (((ItemStates.ItemsID)items[i]) == ItemStates.ItemsID.Default)
                {
                    itemTexture = new Texture2D(256, 256);
                }
                else
                {
                    itemTexture = Resources.Load<Sprite>($"InventoryItems\\{(ItemStates.ItemsID)items[i]}").texture;
                }
                var height = GUILayout.MaxHeight(position.height / 12.5f);
                var width = GUILayout.MaxWidth(position.width / 5);
                if (GUILayout.Button(itemTexture, height, width))
                {
                    container.RemoveStartedItem(i);
                    i--;
                    continue;
                }

                string cCount = count[i].ToString();
                string outCount = GUILayout.TextField(cCount.ToString(), width);
                int newCount = 1;
                if (!string.IsNullOrEmpty(outCount))
                    newCount = Convert.ToInt32(outCount);
                newCount = Mathf.Clamp(newCount, 1, ItemStates.GetMaxCount(items[i]));
                container.SetStartedCount(i, newCount);
                EditorGUILayout.EndVertical();
                if ((i + 1) % 5 == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }
            catch (IndexOutOfRangeException)
            {
                Debug.Log($"Null references on InventoryItems\\{(ItemStates.ItemsID)items[i]}");
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }
#endif
}
