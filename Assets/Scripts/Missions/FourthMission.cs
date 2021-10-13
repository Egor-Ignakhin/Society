using Society.Effects;

using UnityEngine;

namespace Society.Missions
{
    sealed class FourthMission : Mission
    {
        public override int GetMissionNumber() => 4;

        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (currentTask == 1)
            {
                
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);

                FinishMission();

                Destroy(db.gameObject, 3);
            }
        }
    }
}