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
    public sealed class ThirdMission : Mission
    {
        [SerializeField] private HermeticDoor hermeticDoor;

        public override int GetMissionNumber() => 3;
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
                SceneManager.LoadScene(ScenesManager.Map);
            });
            base.StartMission();
        }
        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (currentTask == 2)
            {
                MissionsManager.Instance.GetTaskDrawer().SetVisible(false);
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);
                db.SubsctibeOnFinish(OnTaskActions["1"]);
            }                
        }
    }
}