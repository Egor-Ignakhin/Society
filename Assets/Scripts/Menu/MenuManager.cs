using System;

using UnityEngine;

namespace Society.Menu
{
    /// <summary>
    /// Базовый класс любого меню в игре.
    /// </summary>
    public abstract class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// Общая панель кнопок
        /// </summary>
        [SerializeField] protected GameObject genericPanel;


        /// <summary>
        /// Источник звука меню
        /// </summary>
        private AudioSource menuAudioSource;

        /// <summary>
        /// Клип, вызываемый при наведении курсора на кнопку 
        /// </summary>
        private AudioClip OnButtonEnterSound;

        private void Awake()
        {
            OnButtonEnterSound = Resources.Load<AudioClip>("Inventory\\tic_2");
            menuAudioSource = gameObject.AddComponent<AudioSource>();
            menuAudioSource.volume = 0.5f;

            OnInit();
        }
        internal void ShowGenericPanel() => genericPanel.SetActive(true);

        internal void OnButtonEnter() => menuAudioSource.PlayOneShot(OnButtonEnterSound);
        protected abstract void OnInit();
    }
}