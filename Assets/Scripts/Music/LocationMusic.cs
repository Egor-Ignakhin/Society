using System.Collections;

using Society.Player.Controllers;

using UnityEngine;
namespace Society.Music
{
    internal sealed class LocationMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip[] musicList;
        [SerializeField] private float musicWaitTime = 5f;
        private Collider playerCollider;
        private Coroutine startMusic;
        private bool canPlayMusic;
        private AudioSource audioSource;
        private int randomMusicNumber;
        private int previousMusicNumber = -1;
        private bool musicIsEnabled = true;

        [SerializeField] private bool isNonFPCScene = false;

        [SerializeField, Range(0, 1)] private float sourceVolume;
        private void Start()
        {
            var fpc = FindObjectOfType<FirstPersonController>();
            if (fpc)
                playerCollider = fpc.GetCollider();

            audioSource = gameObject.GetComponent<AudioSource>(); // Вешаем скрипт и AudioSource на объект, где будет идти музыка
            audioSource.priority = 0;
            MusicFlag();
            MusicPlaying();

            Settings.GameSettings.UpdateSettingsEvent += OnSettingsUpdate;
        }
        private void OnTriggerStay(Collider other) // Проверка вошел ли персонаж в зону действия триггера для включения музыки
        {
            if (other == playerCollider)
                canPlayMusic = true;
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == playerCollider)
                canPlayMusic = false;
        }
        private void Update()
        {
            MusicFlag();
            MusicPlaying();
        }
        private void MusicPlaying()
        {
            if (!audioSource.isPlaying && MusicFlag())
            {
                startMusic = StartCoroutine(SetNewMusic());
            }
        }
        public void SetEnabledMusic(bool v) => musicIsEnabled = v;
        private bool MusicFlag()
        {// Проверка 2ух положений, вошел ли персонаж в триггер зону и включено ли в настройках разрешение на воспроизведение музыки (если нет, то выполняются действия ниже)
            if (((!canPlayMusic) || (!musicIsEnabled)) && (!isNonFPCScene))
            {
                audioSource.Stop();
                audioSource.enabled = false;
                return false;
            }
            else // Иначе запускаем музыку
            {
                audioSource.enabled = true;
                return true;
            }
        }
        private IEnumerator SetNewMusic()
        {
            while (randomMusicNumber == previousMusicNumber)
            {
                randomMusicNumber = Random.Range(0, musicList.Length);
            }
            yield return new WaitForSeconds(musicWaitTime);
            previousMusicNumber = randomMusicNumber;
            audioSource.clip = musicList[randomMusicNumber];
            audioSource.Play();
            StopCoroutine(startMusic);
        }

        private void OnSettingsUpdate()
        {
            audioSource.volume = (float)(sourceVolume * Settings.GameSettings.GetMusicVolume());
        }
        private void OnDestroy()
        {
            Settings.GameSettings.UpdateSettingsEvent -= OnSettingsUpdate;
        }
    }
}