
using System.IO;

using Society.Missions;

using UnityEditor;

using UnityEngine;

namespace Society.Editor
{
    internal class GUIMissionsHelper : EditorWindow
    {
        [MenuItem("Window/GUI Missions Helper")]
        public static void ShowWindow()
        {
            GetWindow(typeof(GUIMissionsHelper));
        }

        private void OnGUI()
        {
            if (focusedWindow != this)
                return;

            if (Application.isPlaying)
                return;

            autoRepaintOnSceneChange = true;

            titleContent.text = $"Missions Helper";

            GUILayout.BeginVertical();

            var currentState = Missions.MissionsManager.LoadState();

            Missions.MissionsManager.MissionInfo.UpdateInfoAboutMissions();
            string cmTitle = MissionsManager.MissionInfo.GetMissionTitleByIndex(currentState.currentMission);
            string ctTitle = MissionsManager.MissionInfo.GetMissionTaskTitleByIndex(currentState.currentMission, currentState.currentTask);

            EditorGUILayout.LabelField("Current mission:");
            EditorGUILayout.LabelField("    ID - " + currentState.currentMission);
            EditorGUILayout.LabelField("    Title - " + cmTitle);
            currentState.currentMission = EditorGUILayout.IntSlider(currentState.currentMission, MissionsManager.MinMissions, MissionsManager.MaxMissions);

            EditorGUILayout.LabelField("Current task:");
            EditorGUILayout.LabelField("    ID - " + currentState.currentTask);
            EditorGUILayout.LabelField("    Title - " + ctTitle);
            currentState.currentTask = EditorGUILayout.IntSlider(currentState.currentTask, 0, MissionsManager.MissionInfo.GetMaxTasksByIndex(currentState.currentMission) - 1);

            Missions.MissionsManager.SaveState(currentState);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Reset missions"))
                ResetMissions();

            if (GUILayout.Button("Reset tasks"))
                ResetTasks();

            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }


        /// <summary>
        /// —брос текущей миссии до 0 и еЄ задачи тоже
        /// </summary>
        private void ResetMissions()
        {
            MissionsManager.State state = MissionsManager.LoadState();
            state.currentMission = MissionsManager.MinMissions;
            state.currentTask = 0;

            string data = JsonUtility.ToJson(state, true);
            File.WriteAllText(MissionsManager.SavePath, data);
        }

        /// <summary>
        /// —брос текущей задачи до 0
        /// </summary>
        private void ResetTasks()
        {
            MissionsManager.State state = MissionsManager.LoadState();
            state.currentTask = 0;

            string data = JsonUtility.ToJson(state, true);
            File.WriteAllText(MissionsManager.SavePath, data);
        }
    }
}

