namespace Society.Missions
{
    internal static class Plot
    {
        public static int CalculateCompletedPercent()
        {
            var state = MissionsManager.Instance.GetPlotState();

            int value = (int)((float)Localization.LocalizationManager.GetNumberOfCompletedMissionTasks(
                state.currentMission, state.currentTask) / Localization.LocalizationManager.GetSumOfAllTasksOfAllMissions() * 100);

            return value;
        }
    }
}