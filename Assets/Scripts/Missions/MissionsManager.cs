using PlayerClasses;
using System.IO;
using UnityEngine;

public sealed class MissionsManager : MonoBehaviour
{
    private BasicNeeds playerBasicNeeds;
    private EffectsCanvas effectsCanvas;
    private Dialogs.DialogDrawer dialogDrawer;
    private TaskDrawer taskDrawer;
    public static string StateFolder { get; private set; } = Directory.GetCurrentDirectory() + "\\Saves";// папка с сохранением
    public static string StateFile { get; private set; } = "\\State.json";// сохранение
    private State currentState;

    private string nextMission;
    public enum MissionType
    {
        none,
        narrative,
        additional    
    }
    private void OnEnable()
    {
        taskDrawer = FindObjectOfType<TaskDrawer>();
        dialogDrawer = FindObjectOfType<Dialogs.DialogDrawer>();
        effectsCanvas = FindObjectOfType<EffectsCanvas>();
        playerBasicNeeds = BasicNeeds.Instance;
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
            string data = File.ReadAllText(StateFolder + StateFile);
            currentState = JsonUtility.FromJson<State>(data);
        }
        catch
        {
            currentState = new State();
            if (!Directory.Exists(StateFolder))
            {
                Directory.CreateDirectory(StateFolder);
                File.Create(StateFolder + StateFile);
            }
        }
    }

    internal EffectsCanvas GetEffectsCanvas()
    {
        return effectsCanvas;
    }
    internal Dialogs.DialogDrawer GetDialogDrawer()
    {
        return dialogDrawer;
    }
    internal TaskDrawer GetTaskDrawer()
    {
        return taskDrawer;
    }

    private void SaveState()
    {
        string data = JsonUtility.ToJson(currentState, true);
        File.WriteAllText(StateFolder + StateFile, data);
    }
    public void ResetTasks()
    {
        currentState.currentTask = 0;
    }
    public void ReportMission()
    {
        currentState.currentMission++;
        ResetTasks();
    }
    public void ReportTask()
    {
        currentState.currentTask++;
    }
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
    public BasicNeeds GetPlayerBasicNeeds()
    {
        return playerBasicNeeds;
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
        // игрок где-то просыпается или появляется    
        nextMission = "Направляйтесь куда-то на поверхность к дяде Биллу";
        ChangeMissionType(MissionType.none);
        ReportMission();
    }
    private void ChangeMissionType(MissionType type)
    {
        currentState.missionType = type;

        if(type == MissionType.none)// если миссия закончилась и новая не началась
        {
            taskDrawer.DrawNewTask(nextMission);
        }
    }
}
