using UnityEngine;

public abstract class Mission : MonoBehaviour
{
    protected static int currentTask = 0;
    public abstract void Report();
    public abstract int GetMissionNumber();
    public abstract void ContinueMission(int skipLength);
    public abstract void FinishMission();  
    
}