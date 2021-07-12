public sealed class SecondMission : Mission
{
    public override int GetMissionNumber() => 1;

    protected override void OnReportTask(int currentTask, bool isLoad = false)
    {
        throw new System.NotImplementedException();
    }
}
