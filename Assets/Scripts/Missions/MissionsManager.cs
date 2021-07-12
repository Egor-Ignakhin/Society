using System.IO;
using UnityEngine;

public sealed class MissionsManager : MonoBehaviour
{
    public static string savePath = Directory.GetCurrentDirectory() + "\\Saves\\State.json";// папка с сохранением    
    private State currentState;

    public enum MissionType
    {
        none,
        narrative,
        additional
    }
    private void Awake()
    {
        Localization.Init();
    }
    private void OnEnable()
    {
        LoadState();
        StartOrContinueMission(currentState.currentMission);
    }

    /// <summary>
    /// загрузка состояния миссий
    /// </summary>
    private void LoadState()
    {
        try
        {
            string data = File.ReadAllText(savePath);
            currentState = JsonUtility.FromJson<State>(data);
        }
        catch
        {
            currentState = new State();
            if (!File.Exists(savePath))                            
                File.Create(savePath);            
        }
    }
    private void SaveState()
    {
        string data = JsonUtility.ToJson(currentState, true);
        File.WriteAllText(savePath, data);
    }
    public void ResetTasks() => currentState.currentTask = 0;

    public void ReportMission()
    {
        currentState.currentMission++;
        ResetTasks();
    }
    public void ReportTask() => currentState.currentTask++;

    public void StartOrContinueMission(int num)
    {
        switch (num)
        {
            case 0:
                FindObjectOfType<FirstMission>().ContinueMission(currentState.currentTask);
                ChangeMissionType(MissionType.narrative);
                break;
        }
    }
    private void OnDisable()
    {
        SaveState();
    }
    [System.Serializable]
    public class State
    {
        public MissionType missionType = MissionType.none;
        public int currentMission = 0;
        public int currentTask = 0;
    }
    public void FinishMission(int n)
    {
        switch (n)
        {
            case 0:
                Finish1Mission();
                break;
        }
    }
    private void Finish1Mission()
    {
        ChangeMissionType(MissionType.none);
        ReportMission();
    }
    private void ChangeMissionType(MissionType type) =>
        currentState.missionType = type;

}
