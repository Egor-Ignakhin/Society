using System.IO;
using UnityEditor;
using UnityEngine;

public class ReseterMissions
{
#if UNITY_EDITOR
    [MenuItem("Tools/Reset missions")]
    private static void ResetMissions()
    {
        MissionsManager.State state = new MissionsManager.State();
        string data = JsonUtility.ToJson(state, true);
        File.WriteAllText(MissionsManager.savePath, data);
    }
#endif
}
