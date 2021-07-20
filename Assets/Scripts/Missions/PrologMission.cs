using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Missions
{
    public sealed class PrologMission : Mission
    {
        public override int GetMissionNumber() => 0;
        [SerializeField] private BedMesh onLoadBedMesh;
        private readonly List<Action> actions = new List<Action>();
        [SerializeField] private SanSanychPerson sanSanych;
        [SerializeField] private IlyaiPerson ilya;
        [SerializeField] private GameObject ilyaObjects;
        protected override void Awake()
        {
            actions.Add(() =>
            {
                FindObjectOfType<BedController>().SetPossibleDeoccupied(true);
                Inventory.DescriptionDrawer.Instance.SetIrremovableHint($"Чтобы встать нажмите '{FindObjectOfType<BedController>().HideKey()}' ");
            });
            actions.Add(() =>
            {
                SceneManager.LoadScene(ScenesManager.MainMenu);
            });
            base.Awake();
        }
        protected override void OnReportTask(int currentTask, bool isLoad = false, bool isMissiomItem = false)
        {
            if (isLoad)
            {
                FindObjectOfType<MapOfWorldCanvas>().SetVisible(false);
                FindObjectOfType<Inventory.InventoryContainer>().SetInteractive(false);
                FindObjectOfType<Inventory.InventoryContainer>().ClearInventory();
                FindObjectOfType<PlayerActionBar>().SetVisible(false);
                PlayerClasses.BasicNeeds.Instance.SetEnableStamins(false);
                Times.WorldTime.CurrentDate.ForceSetTime("23:32");
            }
            if (isMissiomItem)
            {
                if (currentTask == 5)
                {
                    if (missionItems == 3)
                    {
                        Report();
                    }
                }
                return;
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
                sanSanych.Say(Resources.Load<AudioClip>("Dialogs\\Other\\SanSanych\\0"));
            }
            if (currentTask == 2)
            {
                TaskDrawer.Instance.SetVisible(false);
            }
            if (currentTask == 3)
            {
                TaskDrawer.Instance.SetVisible(true);
            }
            if (currentTask == 4)
            {
                TaskDrawer.Instance.SetVisible(false);
                ilya.PlayDialogsTraker();
            }
            if (currentTask == 5)
            {
                ilyaObjects.SetActive(true);
            }
            if (currentTask == 7)
            {
                //завхоз бежит
                sanSanych.SetRunningState(true);
                sanSanych.SetTarget(FindObjectOfType<FirstPersonController>().transform);
            }
            if (currentTask == 8)
            {
                FindObjectOfType<Inventory.InventoryContainer>().AddItem((int)Inventory.ItemStates.ItemsID.Tablets_1, 10, new SMGInventoryCellGun());
            }
            if (currentTask == 9)
            {
                TaskDrawer.Instance.SetVisible(false);
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);
                db.SubsctibeOnFinish(actions[1]);
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
}