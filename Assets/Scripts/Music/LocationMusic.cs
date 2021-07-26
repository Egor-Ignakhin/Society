using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace Effects
{
    public sealed class LocationMusic : MonoBehaviour
    {
        [SerializeField] private AudioClip[] musicList;
        [SerializeField] private float musicWaitTime = 5f;
        private Collider playerCollider;
        private Coroutine startMusic;
        private Slider musicSlider;
        private Toggle musicToogle;
        private bool canPlayMusic;
        private AudioSource audioSource;
        private int randomMusicNumber;
        private int previousMusicNumber = -1;
        private void Start()
        {
            playerCollider = FindObjectOfType<FirstPersonController>().GetCollider();
            musicSlider = GameObject.Find("[Canvas] PauseMenu(Clone)").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(4).GetChild(1).GetComponent<Slider>(); // Slider в меню PauseMenu, который отвечает за гомкость включенной музыки
            musicToogle = GameObject.Find("[Canvas] PauseMenu(Clone)").transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(4).GetChild(0).GetComponent<Toggle>(); // Toggle в меню PauseMenu, котоырй отвечает за включение\отключение музыки
            audioSource = gameObject.GetComponent<AudioSource>(); // Вешаем скрипт и AudioSource на объект, где будет идти музыка
            MusicFlag();
            MusicPlaying();
        }
        private void OnTriggerEnter(Collider other) // Проверка вошел ли персонаж в зону действия триггера для включения музыки
        {
            if (other == playerCollider)
            {
                canPlayMusic = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other == playerCollider)
            {
                canPlayMusic = false;
            }
        }
        private void Update()
        {
            MusicFlag();
            MusicPlaying();
        }
        private void MusicPlaying()
        {
            if (!audioSource.isPlaying && MusicFlag() == true)
            {
                startMusic = StartCoroutine(SetNewMusic());
            }
            audioSource.volume = musicSlider.value;
        }
        private bool MusicFlag()
        {
            if (!canPlayMusic || !musicToogle.isOn) // Проверка 2ух положений, вошел ли персонаж в триггер зону и включено ли в настройках разрешение на воспроизведение музыки (если нет, то выполняются действия ниже)
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
    }
}