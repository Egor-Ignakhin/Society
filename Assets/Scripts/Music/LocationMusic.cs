using UnityEngine;
using System.Collections;

sealed class LocationMusic : MonoBehaviour
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
    private void Start()
    {
        playerCollider = FindObjectOfType<FirstPersonController>().GetCollider();
        audioSource = gameObject.GetComponent<AudioSource>(); // Вешаем скрипт и AudioSource на объект, где будет идти музыка
        audioSource.priority = 0;
        MusicFlag();
        MusicPlaying();
    }
    private void OnTriggerStay(Collider other) // Проверка вошел ли персонаж в зону действия триггера для включения музыки
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
        if (!audioSource.isPlaying && MusicFlag())
        {
            startMusic = StartCoroutine(SetNewMusic());
        }
    }
    public void SetVolume(float v) => audioSource.volume = v;
    public void SetEnabledMusic(bool v) => musicIsEnabled = v;
    private bool MusicFlag()
    {
        if ((!canPlayMusic) || (!musicIsEnabled)) // Проверка 2ух положений, вошел ли персонаж в триггер зону и включено ли в настройках разрешение на воспроизведение музыки (если нет, то выполняются действия ниже)
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
