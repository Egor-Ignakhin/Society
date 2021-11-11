using Society.Effects;

using UnityEngine;

namespace Society.Missions.NumeratedMissions
{
    internal sealed class FourthMission : Mission
    {
        public override int GetMissionNumber() => 4;

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
        }
    }
}