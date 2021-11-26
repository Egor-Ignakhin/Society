using System;

using UnityEngine;

namespace Society.Menu
{
    /// <summary>
    /// ������� ����� ������ ���� � ����.
    /// </summary>
    public abstract class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// ����� ������ ������
        /// </summary>
        [SerializeField] protected GameObject genericPanel;


        /// <summary>
        /// �������� ����� ����
        /// </summary>
        private AudioSource menuAudioSource;

        /// <summary>
        /// ����, ���������� ��� ��������� ������� �� ������ 
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