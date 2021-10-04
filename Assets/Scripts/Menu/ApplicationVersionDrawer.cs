#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#endif
using UnityEngine;

namespace Society.Tools
{
    sealed class ApplicationVersionDrawer : MonoBehaviour
    {
        public ApplicationVersion applicationVersion;


#if UNITY_EDITOR
        [PostProcessBuild()]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {            
            ApplicationVersion av = FindObjectOfType<ApplicationVersionDrawer>().applicationVersion;
            av.BuildVersion++;
        }
#endif
        private void Start()
        {
            string text = applicationVersion.ToString();
            GetComponent<TMPro.TextMeshProUGUI>().SetText(text);
        }
    }
}