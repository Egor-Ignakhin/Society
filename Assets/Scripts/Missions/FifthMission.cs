using Society.Effects;
using Society.GameScreens;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Society.Missions
{
    internal sealed class FifthMission : Mission
    {
        public override int GetMissionNumber() => 5;

        protected override void StartMission()
        {
            OnTaskActions.Add("exitToMenu", () =>
            {
                ScreensManager.SetScreen(null);
                SceneManager.LoadScene(ScenesManager.MainMenu);
            });

            base.StartMission();
        }

        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (isLoad)
                taskDrawer.SetVisible(true);
            switch (currentTask)
            {

                case 1:
                    taskDrawer.SetVisible(false);
                    DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                    db.OnInit(2, Color.black);
                    db.SubsctibeOnFinish(OnTaskActions["exitToMenu"]);
                    break;

                default:
                    break;
            }
        }
    }
}