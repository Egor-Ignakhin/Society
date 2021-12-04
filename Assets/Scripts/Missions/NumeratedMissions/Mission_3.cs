using System.Collections.Generic;

using Society.Effects;

using UnityEngine;

namespace Society.Missions.NumeratedMissions
{
    internal sealed class Mission_3 : Mission
    {
        protected override Dictionary<MissionItem, bool> MissionItems { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public override int GetMissionNumber() => 3;

        protected override void StartMission()
        {
            OnTaskActions.Add("OnFinish", () =>
           {
               FinishMission();
           });

            base.StartMission();
        }
        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (currentTask == 1)
            {
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);
                db.SubsctibeOnFinish(OnTaskActions["OnFinish"]);
                Destroy(db.gameObject, 3);

                MissionsManager.Instance.GetTaskDrawer().SetVisible(false);
            }
            base.OnReportTask(isLoad, isMissiomItem);
        }
    }
}