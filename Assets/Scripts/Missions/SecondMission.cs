using UnityEngine;
using UnityEngine.SceneManagement;

namespace Missions
{
    sealed class SecondMission : Mission
    {
        public override int GetMissionNumber() => 2;
        protected override void StartMission()
        {
            OnTaskActions.Add("finish", () =>
            {
                taskDrawer.SetVisible(false);
                DirtyingScreenEffect db = new GameObject(nameof(DirtyingScreenEffect)).AddComponent<DirtyingScreenEffect>();
                db.OnInit(2, Color.black);
                db.SubsctibeOnFinish(OnTaskActions["onLoadBunker"]);
            });
            OnTaskActions.Add("onLoadBunker", () =>
            {
                ScreensManager.SetScreen(null);
                SceneManager.LoadScene(ScenesManager.Bunker);
            });
            base.StartMission();
        }
        protected override void OnReportTask(bool isLoad = false, bool isMissiomItem = false)
        {
            if (currentTask == 4)
            {
                Report();// пока нет дневника - пропуск
            }
            if (currentTask == 6)
            {
                OnTaskActions["finish"].Invoke();
            }
        }
    }
}