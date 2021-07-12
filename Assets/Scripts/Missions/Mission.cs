using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    protected static int currentTask = 0;
    private MissionsManager missionsManager;
    protected virtual void Awake()
    {
        missionsManager = FindObjectOfType<MissionsManager>();
    }
    private void Start()
    {
        OnReportTask(currentTask, true);
    }
    /// <summary>
    /// событие чекпоинта
    /// </summary>
    public void Report()
    {
        missionsManager.ReportTask();
        SetTask(++currentTask);
        OnReportTask(currentTask);
    }

    public abstract int GetMissionNumber();
    public void ContinueMission(int skipLength)
    {     
    
        currentTask = skipLength;
        SetTask(currentTask);
    }

    public  void FinishMission()
    {
        missionsManager.FinishMission(GetMissionNumber());
        gameObject.SetActive(false);
    }
    protected void SetTask(int number)
    {
        string neededContent = Localization.PathToCurrentLanguageContent(Localization.Type.Tasks, GetMissionNumber(), number);

        TaskDrawer.Instance.DrawNewTask(neededContent);
    }
    protected abstract void OnReportTask(int currentTask, bool isLoad = false);
}