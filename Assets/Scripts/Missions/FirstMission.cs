using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FirstMission : Mission
{
    public override int GetMissionNumber() => 0;
    [SerializeField] private BedMesh onLoadBedMesh;
    private readonly List<Action> actions = new List<Action>();
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
