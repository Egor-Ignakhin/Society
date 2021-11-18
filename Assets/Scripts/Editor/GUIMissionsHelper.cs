
using System.IO;

using Newtonsoft.Json;

using Society.Localization;
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

            LocalizationManager.InitializeMissions();

            autoRepaintOnSceneChange = true;

            titleContent.text = $"Missions Helper";

            GUILayout.BeginVertical();

            var currentState = MissionsManager.Instance.GetPlotState();

            LocalizationManager.InitializeMissions();
            string cmTitle = LocalizationManager.GetMissionTitle(currentState.currentMission);
            string ctTitle = LocalizationManager.GetTaskTitle(currentState.currentMission, currentState.currentTask);

            if(ctTitle == "Error")
            {
                ResetTasks();
            }

            EditorGUILayout.LabelField("Current mission:");
            EditorGUILayout.LabelField("    ID - " + currentState.currentMission);
            EditorGUILayout.LabelField("    Title - " + cmTitle);
            currentState.currentMission = EditorGUILayout.IntSlider(currentState.currentMission, MissionsManager.MinMissions, MissionsManager.MaxMissions);

            EditorGUILayout.LabelField("Current task:");
            EditorGUILayout.LabelField("    ID - " + currentState.currentTask);
            EditorGUILayout.LabelField("    Title - " + ctTitle);
            currentState.currentTask = EditorGUILayout.IntSlider(currentState.currentTask, 0, LocalizationManager.GetNumberOfMissionTasks(currentState.currentMission) - 1);

            MissionsManager.SaveState();

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
            PlotState state = MissionsManager.Instance.GetPlotState();
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
            var state = MissionsManager.Instance.GetPlotState();
            state.currentTask = 0;

            string data = JsonConvert.SerializeObject(state, Formatting.Indented);
            File.WriteAllText(MissionsManager.SavePath, data);
        }
    }
}

