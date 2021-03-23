using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    protected int currentTask = 0;
    public abstract void Report();
    public abstract int GetMissionNumber();
    public abstract void StartOrContinueMission(int skipLength);
    public abstract void FinishMission();  
    
}