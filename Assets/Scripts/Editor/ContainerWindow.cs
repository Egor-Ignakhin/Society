using System;
using UnityEditor;
using UnityEngine;
using Inventory;
using System.Collections.Generic;
using System.Linq;

public sealed class ContainerWindow : EditorWindow
{

#if UNITY_EDITOR
    private static int item = -1;
    
    private Vector2 verticalScrollPosition;
    [MenuItem("Window/GUI Containers Data")]
    public static void ShowWindow()
    {
        item = -1;
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
        (List<int> items, List<int> count, List<SMGInventoryCellGun> possibleGuns) = container.GetStartedData();
        DrawFreeItems(items.Count, container);

        DrawItemEditButtons(container, items, count, possibleGuns);

        DrawContentCount(items.Count);

        DrawCurrentContent(items);
    }

    private void DrawFreeItems(int currentCount, ItemsContainer container)
    {
        EditorGUILayout.LabelField($"All items");
        EditorGUILayout.BeginHorizontal();
        for (int i = 1; i < Enum.GetNames(typeof(ItemStates.ItemsID)).Length; i++)
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
        GUI.contentColor = count < ItemsContainer.maxCells ? Color.white : Color.red;

        EditorGUILayout.LabelField($"Content used [{count} / {ItemsContainer.maxCells}]");
        GUI.contentColor = Color.white;
    }
    private void DrawItemEditButtons(ItemsContainer container, List<int> items, List<int> count, List<SMGInventoryCellGun> possibleGuns)
    {
        if (item == -1)
            return;
        var width = GUILayout.MaxWidth(position.width / 5);
        GUILayout.Space(25);
        GUILayout.Label($"Selected item: {(ItemStates.ItemsID)items[item]} :: {item}");
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();

        GUILayout.Label("Count", width);

        string cCount = count[item].ToString();
        string outCount = GUILayout.TextField(cCount.ToString(), width);
        int newCount = 1;

        if (!string.IsNullOrEmpty(outCount))
        {
            int.TryParse(string.Join("", outCount.Where(c => char.IsDigit(c))), out int value);
            newCount = value;
        }
        newCount = Mathf.Clamp(newCount, 1, ItemStates.GetMaxCount(items[item]));
        container.SetStartedCount(item, newCount);

        GUILayout.EndVertical();
        if (ItemStates.ItsGun(items[item]))
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Aim", width);
            GUILayout.TextField("0", width);
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Silencer", width);
            string newSilencerV= GUILayout.TextField(possibleGuns[item].Silencer.ToString(), width);

            container.SetSilencerIndex(item,Convert.ToInt32(newSilencerV));
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            GUILayout.Label("Mag", width);
            GUILayout.TextField("0", width);
            GUILayout.EndVertical();
        }
        GUI.contentColor = Color.red;
        if (GUILayout.Button("Remove item"))
        {
            container.RemoveStartedItem(item);
            item = -1;
        }
        GUI.contentColor = Color.white;
        GUILayout.EndHorizontal();
        GUILayout.Space(25);
    }
    private void DrawCurrentContent(List<int> items)
    {
        verticalScrollPosition = EditorGUILayout.BeginScrollView(verticalScrollPosition, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < items.Count; i++)
        {
            EditorGUILayout.BeginVertical();
            Texture2D itemTexture;
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
                item = i;
            }
            EditorGUILayout.EndVertical();
            if ((i + 1) % 5 == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }
#endif
}
