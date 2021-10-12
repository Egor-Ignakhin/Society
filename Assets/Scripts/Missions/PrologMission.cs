using Society.Dialogs;
using Society.Effects;
using Society.Effects.MapOfWorldCanvasEffects;
using Society.Enviroment.Bed;
using Society.GameScreens;
using Society.Inventory;
using Society.Music;
using Society.Player;
using Society.Player.Controllers;

using UnityEngine;
using UnityEngine.SceneManagement;
namespace Society.Missions
{
    public sealed class PrologMission : Mission
    {
        public override int GetMissionNumber() => 1;
        [SerializeField] private BedMesh onLoadBedMesh;
        [SerializeField] private SanSanychPerson sanSanych;
        [SerializeField] private IlyaiPerson ilya;
        [SerializeField] private GameObject ilyaObjects;
        [SerializeField] private Enviroment.Doors.HermeticDoor hermeticDoor;
        protected override void StartMission()
        {
            OnTaskActions.Add("0", () =>
             {
                 FindObjectOfType<BedController>().SetPossibleDeoccupied(true);
                 missionsManager.DescriptionDrawer.SetIrremovableHint($"Чтобы встать нажмите '{FindObjectOfType<BedController>().HideKey()}' ");
                 FindObjectOfType<FirstPersonController>().StepEventIsEnabled = true;
             });
            OnTaskActions.Add("1", () =>
             {
                 missionsManager.FinishMission();
                 hermeticDoor.Interact(true, OnTaskActions["LoadMap"]);
                 FindObjectOfType<LocationMusic>().SetEnabledMusic(false);
             });
            OnTaskActions.Add("LoadMap", () =>
            {
                ScreensManager.SetScreen(null);
                SceneManager.LoadScene(ScenesManager.Map);
            });
            base.StartMission();
        }
        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (isLoad)
            {
                FindObjectOfType<MapOfWorldCanvas>().SetVisible(false);
                FindObjectOfType<InventoryContainer>().SetInteractive(false);
                FindObjectOfType<InventoryContainer>().ClearInventory();
                FindObjectOfType<PlayerActionBar>().SetVisible(false);
                BasicNeeds.Instance.SetEnableStamins(false);
                Times.WorldTime.CurrentDate.ForceSetTime("23:32");
                FindObjectOfType<FirstPersonController>().SetPossibleSprinting(false);
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
                FindObjectOfType<FirstPersonController>().StepEventIsEnabled = false;
                СleansingScreenEffect lb = new GameObject(nameof(СleansingScreenEffect)).AddComponent<СleansingScreenEffect>();
                lb.OnInit(6, Color.black);
                lb.SubsctibeOnFinish(OnTaskActions["0"]);

                taskDrawer.SetVisible(false);

                //  onLoadBedMesh.Interact();
                //       FindObjectOfType<BedController>().SetPossibleDeoccupied(false);
            }
            if (currentTask == 1)
            {
                sanSanych.Say(Resources.Load<AudioClip>("Dialogs\\Other\\SanSanych\\0"));
            }
            if (currentTask == 2)
            {
                taskDrawer.SetVisible(false);
            }
            if (currentTask == 3)
            {
                taskDrawer.SetVisible(true);
            }
            if (currentTask == 4)
            {
                taskDrawer.SetVisible(false);
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
                FindObjectOfType<InventoryContainer>().AddItem((int)ItemStates.ItemsID.Tablets_1, 10, new SMGInventoryCellGun());
            }
            if (currentTask == 9)
            {
                taskDrawer.SetVisible(false);
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);
                db.SubsctibeOnFinish(OnTaskActions["1"]);
            }
        }
        private void Update()
        {
            if ((currentTask == 0) && FindObjectOfType<BedController>().PossibleDeoccupied)
            {
                if (Input.GetKeyDown(FindObjectOfType<BedController>().HideKey()))
                {
                    missionsManager.DescriptionDrawer.SetIrremovableHint(null);
                    taskDrawer.SetVisible(true);
                }
            }
        }
    }
}