
using System.IO;
#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Callbacks;

#endif
using UnityEngine;

namespace Society.ApplicationTools
{
    internal sealed class ApplicationVersionDrawer : MonoBehaviour
    {
        public static string PathToAppInfo = Directory.GetCurrentDirectory() + "\\Saves\\AppInfo.json";

#if UNITY_EDITOR
        [PostProcessBuild()]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            //Увеличиваем индекс на единицу после каждой сборки.
            string inputData = File.ReadAllText(PathToAppInfo);
            var appInfo = JsonUtility.FromJson<ApplicationVersion>(inputData);
            appInfo.BuildVersion++;

            var outputData = JsonUtility.ToJson(appInfo);
            File.WriteAllText(PathToAppInfo, outputData);
        }
#endif

        private void Start()
        {
            string data = File.ReadAllText(PathToAppInfo);
            string text = JsonUtility.FromJson<ApplicationVersion>(data).ToString();
            GetComponent<TMPro.TextMeshProUGUI>().SetText(text);
        }
    }
}