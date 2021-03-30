using System.IO;
using UnityEngine;

public sealed class MissionsManager : MonoBehaviour
{
    [SerializeField] private BasicNeeds playerBasicNeeds;
    [SerializeField] private Canvas effectsCanvas;
    [SerializeField] private Dialogs.DialogDrawer dialogDrawer;
    [SerializeField] private TaskDrawer taskDrawer;
    public static string StateFolder { get; private set; } = Directory.GetCurrentDirectory() + "\\Saves";// папка с сохранением
    public static string StateFile { get; private set; } = "\\State.json";// сохранение
    private State currentState;
    
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

    internal Canvas GetEffectsCanvas()
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
        public int currentMission = 0;
        public int currentTask = 0;
    }
}
