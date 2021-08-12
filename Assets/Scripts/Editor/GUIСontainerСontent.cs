using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Inventory;
using System.Collections.Generic;
using System.Linq;
namespace Tools
{
    sealed class GUIСontainerСontent : EditorWindow
    {

#if UNITY_EDITOR
        private static int item = -1;

        private Vector2 verticalScrollPosition;
        [MenuItem("Window/GUI Сontainer Сontent")]
        public static void ShowWindow()
        {
            item = -1;
            GetWindow(typeof(GUIСontainerСontent));
        }
        private void OnLostFocus()
        {
            item = -1;
        }

        private void OnGUI()
        {
            if (Application.isPlaying)
            {
                OnLostFocus();
                return;
            }
            autoRepaintOnSceneChange = true;
            UnityEngine.Object selectedObject = Selection.activeObject;
            if (!selectedObject)
                return;
            var obj = selectedObject as GameObject;

            if (!obj)
                return;

            if (!obj.TryGetComponent<ItemsContainer>(out var container))
                return;

            EditorGUILayout.LabelField($"Container     {Selection.activeObject.name}");
            (List<int> items, List<int> count, List<int> aims, List<int> mags, List<int> silencers) = container.GetStartedData();
            DrawFreeItems(items.Count, container);

            DrawItemEditButtons(container, items, count, aims, mags, silencers);

            DrawContentCount(items.Count);

            DrawCurrentContent(items);
        }

        private void DrawFreeItems(int currentCount, ItemsContainer container)
        {
            EditorGUILayout.LabelField($"All items");
            EditorGUILayout.BeginHorizontal();
            for (int i = 0; i < 13; i++)
            {
                if (i == 0)
                    continue;

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
        private void DrawItemEditButtons(ItemsContainer container, List<int> items, List<int> count, List<int> aims, List<int> mags, List<int> silencers)
        {
            void DrawSMGMode(string title, GUILayoutOption smgwidth, ref List<int> mod, Action<int, int> setter)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(title, smgwidth);
                string newSMGV = GUILayout.TextField(mod[item].ToString(), smgwidth);
                if (!string.IsNullOrEmpty(newSMGV))
                    setter(item, Convert.ToInt32(newSMGV));
                else
                    setter(item, 0);
                GUILayout.EndVertical();
            }

            if (item == -1)
                return;
            var width = GUILayout.MaxWidth(position.width / 5);
            GUILayout.Space(25);
            GUILayout.Label($"Selected item:: {item}");
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
                DrawSMGMode("Aim", width, ref aims, container.SetStartedAimIndex);
                DrawSMGMode("Silencer", width, ref silencers, container.SetStartedSilencerIndex);
                DrawSMGMode("Mag", width, ref mags, container.SetStartedMagIndex);
            }
            else
            {
                container.SetStartedAimIndex(item, 0);
                container.SetStartedSilencerIndex(item, 0);
                container.SetStartedMagIndex(item, 0);
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
                try
                {
                    EditorGUILayout.BeginVertical();
                    Texture2D itemTexture = Resources.Load<Sprite>($"InventoryItems\\{(ItemStates.ItemsID)items[i]}").texture;

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
                catch
                {
                    Debug.Log($"InventoryItems\\{(ItemStates.ItemsID)items[i]}");
                }

            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
#endif
    }
}