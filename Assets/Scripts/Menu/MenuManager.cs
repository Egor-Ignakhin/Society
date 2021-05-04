using System.IO;
using UnityEngine;

namespace MenuScripts
{
    public sealed class MenuManager : MonoBehaviour
    {
        public void LoadGame()
        {
            FindObjectOfType<ScenesManager>().LoadNextScene();
        }
        public void EnableInfo()
        {
            ResetMissions();
        }
        public void ResetMissions()
        {
            MissionsManager.State state = new MissionsManager.State();
            string data = JsonUtility.ToJson(state, true);
            File.WriteAllText(MissionsManager.StateFolder + MissionsManager.StateFile, data);
        }

        public void LoadDemoMouseAnton()
        {
            FindObjectOfType<ScenesManager>().LoadNextScene(5);
        }
        public void LoadPolygon()
        {
            FindObjectOfType<ScenesManager>().LoadNextScene(6);
        }
        public void ExitFromGame()
        {
            Application.Quit();
        }
    }
}