using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionItem : InteractiveObject
{
    [SerializeField] private string startedType;
    private void Start()
    {
        SetType(startedType);
    }
    public override void Interact()
    {
        MissionsManager.GetCurrentMission().OnAddMissionItem();
        gameObject.SetActive(false);
    }
}
