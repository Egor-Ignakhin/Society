using Society.Effects;
using Society.Enviroment.Doors;
using Society.GameScreens;
using Society.Music;
using Society.Player.Controllers;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Society.Missions.NumeratedMissions
{
    internal sealed class Mission_2 : Mission
    {
        [SerializeField] private HermeticDoor hermeticDoor;

        protected override Dictionary<MissionItem, bool> MissionItems { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int GetMissionNumber() => 2;
        protected override void StartMission()
        {
            OnTaskActions.Add("1", () =>
            {
                MissionsManager.Instance.FinishMission();
                hermeticDoor.Interact(true, OnTaskActions["LoadMap"]);
                FindObjectOfType<LocationMusic>().SetEnabledMusic(false);
            });
            OnTaskActions.Add("LoadMap", () =>
            {
                ScreensManager.SetScreen(null);
                LoadScreensManager.Instance.LoadScene((int)Scenes.Map);
            });
            base.StartMission();
        }
        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (currentTask == 2)
            {
                //MissionsManager.Instance.TaskDrawer.SetVisible(false);
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);
                db.SubsctibeOnFinish(OnTaskActions["1"]);
            }
            base.OnReportTask(isLoad, isMissiomItem);
        }
    }
}