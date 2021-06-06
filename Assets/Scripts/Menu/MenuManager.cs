using System;
using System.IO;
using UnityEngine;

namespace MenuScripts
{
    public sealed class MenuManager : MonoBehaviour
    {
        private AudioClip OnButtonEnterSound;
        private AudioSource aud;
        private void Awake()
        {
            OnButtonEnterSound = Resources.Load<AudioClip>("Inventory\\tic_2");
            aud = gameObject.AddComponent<AudioSource>();
            aud.volume = 0.5f;
        }
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

        internal void OnButtonEnter()
        {
            aud.PlayOneShot(OnButtonEnterSound);
        }

        public void LoadDemoMouseAnton()
        {
          LoadScreensManager.Instance.LoadLevel(5, 0);
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