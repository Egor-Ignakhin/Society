using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class PrologMission : Mission
{
    public override int GetMissionNumber() => 0;
    [SerializeField] private BedMesh onLoadBedMesh;
    private readonly List<Action> actions = new List<Action>();
    [SerializeField] private TalkingPerson sanSanych;
    protected override void Awake()
    {
        actions.Add(() =>
        {
            FindObjectOfType<BedController>().SetPossibleDeoccupied(true);
            Inventory.DescriptionDrawer.Instance.SetIrremovableHint($"Чтобы встать нажмите '{FindObjectOfType<BedController>().HideKey()}' ");

        });
        base.Awake();
    }
    protected override void OnReportTask(int currentTask, bool isLoad = false)
    {
        if (isLoad)
        {
            FindObjectOfType<MapOfWorldCanvas>().SetVisible(false);
            FindObjectOfType<Inventory.InventoryContainer>().SetInteractive(false);
            FindObjectOfType<PlayerActionBar>().SetVisible(false);
            PlayerClasses.BasicNeeds.Instance.SetEnableStamins(false);
            Times.WorldTime.CurrentDate.ForceSetTime("23:32");
        }
        if (currentTask == 0)
        {
            СleansingScreenEffect lb = new GameObject(nameof(СleansingScreenEffect)).AddComponent<СleansingScreenEffect>();
            lb.OnInit(6, Color.black);
            lb.SubsctibeOnFinish(actions[0]);

            FindObjectOfType<BedController>().SetPossibleDeoccupied(false);

            TaskDrawer.Instance.SetVisible(false);

            onLoadBedMesh.Interact();
        }
        if (currentTask == 1)
        {
            sanSanych.Say(Resources.Load<AudioClip>("Dialogs\\Other\\SanSanych_0"));
        }
        if (currentTask == 2)
        {
            TaskDrawer.Instance.SetVisible(false);
            sanSanych.PlayDialogsTraker();
        }
        if (currentTask == 3)
        {
            TaskDrawer.Instance.SetVisible(true);
        }
    }
    private void Update()
    {
        if ((currentTask == 0) && FindObjectOfType<BedController>().PossibleDeoccupied)
        {
            if (Input.GetKeyDown(FindObjectOfType<BedController>().HideKey()))
            {
                Inventory.DescriptionDrawer.Instance.SetIrremovableHint(null);
                TaskDrawer.Instance.SetVisible(true);
            }
        }
    }
}
