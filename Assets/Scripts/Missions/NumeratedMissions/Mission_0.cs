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
namespace Society.Missions.NumeratedMissions
{
    public sealed class Mission_0 : Mission
    {
        public override int GetMissionNumber() => 0;
        [SerializeField] private BedMesh onLoadBedMesh;
        [SerializeField] private SanSanychNPC sanSanych;
        [SerializeField] private IlyaiNPC ilya;
        [SerializeField] private GameObject ilyaObjects;
        [SerializeField] private Enviroment.Doors.HermeticDoor hermeticDoor;
        protected override void StartMission()
        {
            OnTaskActions.Add("0", () =>
             {
                 FindObjectOfType<BedController>().SetPossibleDeoccupied(true);
                 MissionsManager.Instance.DescriptionDrawer.SetIrremovableHint($"Чтобы встать нажмите '{FindObjectOfType<BedController>().HideKey()}' ");
                 FindObjectOfType<FirstPersonController>().StepEventIsEnabled = true;
             });
            OnTaskActions.Add("1", () =>
             {                 
                 hermeticDoor.Interact(true, OnTaskActions["LoadMap"]);
                 FindObjectOfType<LocationMusic>().SetEnabledMusic(false);

                 base.FinishMission();
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
                if (currentTask == 3)
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

                MissionsManager.Instance.GetTaskDrawer().SetVisible(false);

                onLoadBedMesh.Interact();
                FindObjectOfType<BedController>().SetPossibleDeoccupied(false);
            }
            if (currentTask == 1)
            {
                sanSanych.Say(Resources.Load<AudioClip>("Dialogs\\Other\\SanSanych\\0"));
            }

            if (currentTask == 3)
            {
                ilyaObjects.SetActive(true);
            }
            if (currentTask == 5)
            {
                //завхоз бежит
                sanSanych.SetRunningState(true);
                sanSanych.SetTarget(FindObjectOfType<FirstPersonController>().transform);
            }
        }
        public override void FinishMission()
        {
            MissionsManager.Instance.GetTaskDrawer().SetVisible(false);
            DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
            db.OnInit(2, Color.black);
            db.SubsctibeOnFinish(OnTaskActions["1"]);            
        }
        private void Update()
        {
            if ((currentTask == 0) && FindObjectOfType<BedController>().PossibleDeoccupied)
            {
                if (Input.GetKeyDown(FindObjectOfType<BedController>().HideKey()))
                {
                    MissionsManager.Instance.DescriptionDrawer.SetIrremovableHint(null);
                    MissionsManager.Instance.GetTaskDrawer().SetVisible(true);
                }
            }
        }
    }
}