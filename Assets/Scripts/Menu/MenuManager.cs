using System;

using UnityEngine;

namespace Society.Menu
{
    public abstract class MenuManager : MonoBehaviour
    {
        [SerializeField] protected GameObject genericPanel;
        private AudioSource aud;
        private AudioClip OnButtonEnterSound;

        private void Awake()
        {
            OnButtonEnterSound = Resources.Load<AudioClip>("Inventory\\tic_2");
            aud = gameObject.AddComponent<AudioSource>();
            aud.volume = 0.5f;

            OnInit();
        }
        internal void ShowGenericPanel()
        {
            genericPanel.SetActive(true);
        }

        internal void OnButtonEnter()
        {
            aud.PlayOneShot(OnButtonEnterSound);
        }
        protected abstract void OnInit();
    }
}