using System.IO;
using UnityEngine;

public sealed class MissionsManager : MonoBehaviour
{
    [SerializeField] private Canvas effectsCanvas;
    [SerializeField] private Dialogs.DialogDrawer dialogDrawer;
    [SerializeField] private TaskDrawer taskDrawer;
    private string stateFolder = Directory.GetCurrentDirectory() + "\\Saves";// папка с сохранением
    private string stateFile = "\\State.json";// сохранение
    private State currentState;
    private void OnEnable()
    {
        LoadState();
        StartMission(currentState.currentMission);
    }
    /// <summary>
    /// загрузка состояния миссий
    /// </summary>
    private void LoadState()
    {
        try
        {
            string data = File.ReadAllText(stateFolder + stateFile);
            currentState = JsonUtility.FromJson<State>(data);

        }
        catch
        {

            currentState = new State();
            if (!Directory.Exists(stateFolder))
            {
                Directory.CreateDirectory(stateFolder);
                File.Create(stateFolder + stateFile);
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
        File.WriteAllText(stateFolder + stateFile, data);
    }
    public void StartMission(int num)
    {
        switch (num)
        {
            case 0:
                FindObjectOfType<FirstMission>().StartMission();
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
        public int currentMission = 0;
    } 
}
