using Society.Effects;
using Society.GameScreens;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Society.Missions.NumeratedMissions
{
    internal sealed class Mission_4 : Mission
    {
        public override int GetMissionNumber() => 4;

        protected override void StartMission()
        {
            OnTaskActions.Add("exitToMenu", () =>
            {
                ScreensManager.SetScreen(null);
                LoadScreensManager.Instance.LoadScene((int)Scenes.MainMenu);
            });

            base.StartMission();
        }

        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (isLoad)
                MissionsManager.Instance.GetTaskDrawer().SetVisible(true);
            switch (currentTask)
            {

                case 1:
                    MissionsManager.Instance.GetTaskDrawer().SetVisible(false);
                    DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                    db.OnInit(2, Color.black);
                    db.SubsctibeOnFinish(OnTaskActions["exitToMenu"]);
                    break;

                default:
                    break;
            }
            base.OnReportTask(isLoad, isMissiomItem);
        }
    }
}