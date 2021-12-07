using System.Collections.Generic;

using Society.Dialogs;
using Society.Effects;
using Society.Effects.MapOfWorldCanvasEffects;
using Society.Enviroment.Bed;
using Society.Enviroment.Doors;
using Society.Features.Sink;
using Society.GameScreens;
using Society.Inventory;
using Society.Missions.TaskSystem;
using Society.Music;
using Society.Player;
using Society.Player.Controllers;

using UnityEngine;
namespace Society.Missions.NumeratedMissions
{
    internal sealed class Mission_0 : Mission
    {
        public override int GetMissionNumber() => 0;
        [SerializeField] private BedMesh onLoadBedMesh;
        [SerializeField] private SanSanychNPC sanSanych;
        [SerializeField] private GameObject ilyaObjects;
        [SerializeField] private HermeticDoor hermeticDoor;

        [SerializeField] private AudioSource mSource;

        [SerializeField] private Transform task_0_place;
        [SerializeField] private DoorManager firstTaskDoorManager;

        [SerializeField] private MissionItem mi0;
        [SerializeField] private MissionItem mi1;
        [SerializeField] private MissionItem mi2;
        [SerializeField] private MissionItem mi3;

        [SerializeField] private SinkInteractiveObject sinkInteractiveObject;
        [SerializeField] private AudioClip gulGeneratorClip;
        private void Awake()
        {
            MissionItems = new Dictionary<MissionItem, bool> { { mi0, false }, { mi1, false }, { mi2, false }, { mi3, false } };

            taskActions = new List<System.Action>
            {
               ()=> { 
                   //firstTaskDoorManager.SetState(Patterns.State.locked);

                mSource.clip = Resources.Load<AudioClip>("Missions\\Mission_0\\mission_0_noise");
                mSource.Play();

                FindObjectOfType<FirstPersonController>().StepEventIsEnabled = false;
                СleansingScreenEffect lb = new GameObject(nameof(СleansingScreenEffect)).AddComponent<СleansingScreenEffect>();
                lb.OnInit(6, Color.black);
                lb.SubsctibeOnFinish(()=>{
                 FindObjectOfType<BedController>().SetPossibleDeoccupied(true);
                 MissionsManager.Instance.DescriptionDrawer.SetIrremovableHint($"Чтобы встать нажмите '{FindObjectOfType<BedController>().HideKey()}' ");
                 FindObjectOfType<FirstPersonController>().StepEventIsEnabled = true;
                });

                MissionsManager.Instance.GetTaskDrawer().SetVisible(false);

                onLoadBedMesh.Interact();
                FindObjectOfType<BedController>().SetPossibleDeoccupied(false);

                FindObjectOfType<FirstPersonController>().SetPosition(task_0_place.position);
                FindObjectOfType<FirstPersonController>().transform.rotation = task_0_place.rotation;
               sinkInteractiveObject.FinishProcedureEvent += () => {Report();}; },                
                () => {mSource.PlayOneShot(gulGeneratorClip); /*sanSanych.Say(Resources.Load<AudioClip>("Dialogs\\Other\\SanSanych\\0"));*/},
                () => { ilyaObjects.SetActive(true);},
                () => { },
                () => { },
                () => { //завхоз бежит
                sanSanych.SetRunningState(true);
                sanSanych.SetTarget(FindObjectOfType<FirstPersonController>().transform);}
            };
        }
        protected override Dictionary<MissionItem, bool> MissionItems { get; set; } = new Dictionary<MissionItem, bool>();

        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (isLoad)
            {
                FindObjectOfType<MapOfWorldCanvas>().SetVisible(false);
                FindObjectOfType<InventoryContainer>().SetInteractive(false);
                FindObjectOfType<InventoryContainer>().ClearInventory();
                FindObjectOfType<PlayerActionBar>().gameObject.SetActive(false);
                BasicNeeds.Instance.SetEnableStamins(false);
                Times.WorldTime.CurrentDate.ForceSetTime("23:32");
                FindObjectOfType<FirstPersonController>().SetPossibleSprinting(false);
            }
            if (isMissiomItem)
            {
                if (currentTask == 0)
                {
                    if (MissionItems[mi0])
                    {
                        // firstTaskDoorManager.SetState(Patterns.State.unlocked);
                    }
                }
                if (currentTask == 3)
                {
                    if (MissionItems[mi1] && MissionItems[mi2] && MissionItems[mi3])
                    {
                        Report();
                    }
                }
                return;
            }

            taskActions[currentTask]?.Invoke();

            base.OnReportTask(isLoad, isMissiomItem);
        }
        public override void FinishMission()
        {
            MissionsManager.Instance.GetTaskDrawer().SetVisible(false);
            DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
            db.OnInit(2, Color.black);
            db.SubsctibeOnFinish(() =>
            {
                hermeticDoor.Interact(true, () =>
                {
                    ScreensManager.SetScreen(null);
                    LoadScreensManager.Instance.LoadScene((int)Scenes.Map);
                });
                FindObjectOfType<LocationMusic>().SetEnabledMusic(false);

                base.FinishMission();
            });
        }
        private void Update()
        {
            if ((currentTask == 0) && FindObjectOfType<BedController>().PossibleDeoccupied)
            {
                if (Input.GetKeyDown(FindObjectOfType<BedController>().HideKey()))
                {
                    MissionsManager.Instance.DescriptionDrawer.SetIrremovableHint(null);
                    MissionsManager.Instance.GetTaskDrawer().SetVisible(true);

                    CommentDrawer.Instance.Push(Localization.LocalizationManager.Translate(Localization.LanguageIdentifiers.Prologue_firstComment));
                }
            }
        }
    }
}