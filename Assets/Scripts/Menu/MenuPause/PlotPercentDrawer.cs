
using UnityEngine;

namespace Society.Menu.MenuPause
{
    internal sealed class PlotPercentDrawer : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI drawableText;

        private void OnEnable()
        {
            string percent = CalculatePercent();
            ReDraw(percent);
        }

        private string CalculatePercent()
        {
            var activeMission = Missions.MissionsManager.Instance.GetActiveMission();
            int activeMissionNumber = activeMission.GetMissionNumber();
            int activeMissionTask = activeMission.GetCurrentTask();

            int value = (int)((float)(activeMissionNumber * activeMissionTask) / (Missions.MissionsManager.MaxMissions * Localization.LocalizationManager.GetNumberOfActiveMissionTasks()) * 100);

            string retPercent = $"{value}%";

            if (retPercent == "00%")
                retPercent = "0%";

            return retPercent;
        }

        private void ReDraw(string percent)
        {
            drawableText.SetText($"Пройдено {percent}");
        }
    }
}