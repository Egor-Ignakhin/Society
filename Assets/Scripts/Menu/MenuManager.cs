using System.IO;
using UnityEngine;

namespace MenuScripts
{
    public sealed class MenuManager : MonoBehaviour
    {
        private AudioClip OnButtonEnterSound;
        private AudioSource aud;
        [SerializeField] private GameObject settings;
        private void Awake()
        {
            OnButtonEnterSound = Resources.Load<AudioClip>("Inventory\\tic_2");
            aud = gameObject.AddComponent<AudioSource>();
            aud.volume = 0.5f;
            settings.SetActive(false);
        }
        public void LoadNewGame()
        {
            Missions.MissionsManager.State state = new Missions.MissionsManager.State();
            string data = JsonUtility.ToJson(state, true);
            File.WriteAllText(Missions.MissionsManager.SavePath, data);

            LoadGame();
        }
        public void Settings()
        {
            settings.gameObject.SetActive(!settings.gameObject.activeInHierarchy);
        }
        public void Keyboard()
        {

        }
        public void LoadGame()
        {
            FindObjectOfType<ScenesManager>().LoadNextScene();
        }
        internal void OnButtonEnter()
        {
            aud.PlayOneShot(OnButtonEnterSound);
        }
        public void ExitFromGame()
        {
            Application.Quit();
        }
    }
}