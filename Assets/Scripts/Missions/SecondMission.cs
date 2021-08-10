namespace Missions
{
    sealed class SecondMission : Mission
    {
        public override int GetMissionNumber() => 1;

        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            throw new System.NotImplementedException();
        }
    }
}