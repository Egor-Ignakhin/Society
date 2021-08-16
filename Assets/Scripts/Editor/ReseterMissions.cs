using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ReseterMissions
{
#if UNITY_EDITOR
    [MenuItem("Tools/Reset missions")]
    private static void ResetMissions()
    {
        Missions.MissionsManager.State state = new Missions.MissionsManager.State();
        string data = JsonUtility.ToJson(state, true);
        File.WriteAllText(Missions.MissionsManager.SavePath, data);
    }
#endif
}
