using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SecondMission : Mission
{
    public override void ContinueMission(int skipLength)
    {
        throw new System.NotImplementedException();
    }

    public override void FinishMission()
    {
        throw new System.NotImplementedException();
    }

    public override int GetMissionNumber()
    {
        return 1;
    }

    public override void Report()
    {
        throw new System.NotImplementedException();
    }
}
