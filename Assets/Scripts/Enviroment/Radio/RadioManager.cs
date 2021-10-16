using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Society.Enviroment.Radio
{
    public sealed class RadioManager : MonoBehaviour
    {
        public enum Action { leftClick, rightClick, activation };

        private bool isActive;
        private decimal currentFrequency = 100;// частота радио
        private decimal minFrequency = 0;// мин. частота
        private decimal maxFrequency = 200;// макс. частота
        private decimal qualityFrequency = 100;// качество звука        
        private AudioSource mAudioSource;// источник звука
        private AudioClip interferenceClip;// клип шумов
        private float interferenceRate = 0.1f;// частота шумов 
        private float additionalInterference = 0.05f;// добавляемые шумы (есть всегда)
        private MediaContainer mMediaContainer;

        private class MediaContainer
        {
            public List<decimal> FrequencyList { get; set; } = new List<decimal>();// частотный лист
            public List<AudioClip> Clips { get; set; } = new List<AudioClip>();// клипы
            public float[] ClipTimes { get; set; } // записи точек остановок клипов
            public MediaContainer(RadioManager rm)
            {
                FrequencyList.Add(100);
                Clips.Add(Resources.Load<AudioClip>("TestAudio1"));


                FrequencyList.Add(101.2m);
                Clips.Add(Resources.Load<AudioClip>("TestAudio2"));

                FrequencyList.Add(98.8m);
                Clips.Add(Resources.Load<AudioClip>("TestAudio3"));

                rm.interferenceClip = Resources.Load<AudioClip>("TestInterferenceClip");

                ClipTimes = new float[FrequencyList.Count];
                for (int i = 0; i < ClipTimes.Length; i++)
                {
                    ClipTimes[i] = 0;
                }
            }
        }
        private void Awake()
        {
            mMediaContainer = new MediaContainer(this);
            mAudioSource = GetComponent<AudioSource>();
        }

        public void SendMessage(Action a)
        {
            switch (a)
            {
                case Action.leftClick:
                    ChangeMusic(false);
                    break;
                case Action.rightClick:
                    ChangeMusic(true);
                    break;
                case Action.activation:
                    Activation();
                    break;
            }
        }
        private void ChangeMusic(bool rightSwipe)// вызывается при нажатии + или - (частота)
        {
            if (!isActive)
                return;

            ChangeFrequency(rightSwipe ? 0.1m : -0.1m);
        }
        private void ChangeFrequency(decimal additionalFr)// функция изменения частоты
        {
            currentFrequency += additionalFr;

            if (currentFrequency > maxFrequency)
                currentFrequency = minFrequency;
            if (currentFrequency < minFrequency)
                currentFrequency = maxFrequency;

            bool isFound = false;
            for (int i = 0; i < mMediaContainer.FrequencyList.Count; i++)
            {
                if (mMediaContainer.FrequencyList[i] == currentFrequency)// если найдена подходящая частота
                {
                    SetAudio(i);

                    qualityFrequency = 100;
                    isFound = true;
                }
            }
            if (!isFound)// если не нашёлся никакой источник сигнала на текущей частоте
            {
                int minMaxNearChannel = 5;
                decimal currentChannel = 0;
                for (int i = 0; i < minMaxNearChannel; i++)
                {
                    if (isFound)
                        break;
                    for (int k = 0; k < mMediaContainer.FrequencyList.Count; k++)
                    {
                        if (mMediaContainer.FrequencyList[k] == currentFrequency + currentChannel ||
                            mMediaContainer.FrequencyList[k] == currentFrequency - currentChannel)
                        {
                            SetAudio(k);

                            isFound = true;
                            qualityFrequency = 100 - i * (100 / minMaxNearChannel);

                            break;
                        }
                    }
                    currentChannel += 0.1m;
                }
            }
            if (!isFound)
            {
                StopAudio();
            }
        }
        private void SetAudio(int it)// устанавливат новый звук
        {
            if (mAudioSource.clip != mMediaContainer.Clips[it])
            {
                mAudioSource.clip = mMediaContainer.Clips[it];
                ReadTime();
                mAudioSource.Play();
            }
        }
        private void StopAudio()// останавливает воспр. звука
        {
            WriteTime();
            mAudioSource.Stop();
            mAudioSource.clip = null;
            qualityFrequency = 0;
        }
        private void WriteTime()// чтение времени из дорожки клипа
        {
            for (int c = 0; c < mMediaContainer.Clips.Count; c++)
            {
                if (mMediaContainer.Clips[c] == mAudioSource.clip)
                {
                    mMediaContainer.ClipTimes[c] = mAudioSource.time;
                }
            }
        }
        private void ReadTime()// запись времени в дорожку клипа
        {
            for (int c = 0; c < mMediaContainer.Clips.Count; c++)
            {
                if (mMediaContainer.Clips[c] == mAudioSource.clip)
                {
                    mAudioSource.time = Mathf.Min(mMediaContainer.ClipTimes[c], mAudioSource.clip.length - 0.01f);
                }
            }
        }
        private IEnumerator PlayBackInterference()// корутина помех
        {
            while (true)
            {
                mAudioSource.PlayOneShot(interferenceClip, (float)Math.Abs(qualityFrequency - 100) / 100 + additionalInterference);
                yield return new WaitForSeconds(interferenceRate);
            }

        }

        private void Activation()// вызывается при смене активности
        {
            isActive = !isActive;
            if (isActive)
            {
                ChangeFrequency(0m);
                StartCoroutine(nameof(PlayBackInterference));
                ReadTime();
            }
            else
            {
                StopAudio();

                StopCoroutine(nameof(PlayBackInterference));
            }
        }
    }
}