using UnityEditor;
using UnityEngine;
/// <summary>
/// класс при вызове создаёт необходимую иерарихю папок
/// </summary>
sealed class StdFoldersCreator
{
#if UNITY_EDITOR
    [MenuItem("Tools/ Create Standart Folders")]
    private static void CreateStdFolders()
    {
        string[] folders = new string[] { "Common", "Enviroment", "Scene", "Camera", "Characters", "UI", "Other" };
        foreach(var f in folders)
        {
            GameObject go = GameObject.Find(f);
            if (!go)
            {
                Debug.Log($"Create STD Folder ===> {f}");
                go = new GameObject(f);
            }
        }
    }
#endif
}
