using System.Collections;
using System.Collections.Generic;

using UnityEngine;
namespace Society.Player
{
    public sealed partial class BasicNeeds : Patterns.Singleton<BasicNeeds>
    {
        private sealed class PlayerSoundEffects : MonoBehaviour
        {
            private const float minHealthForNoise = 20;
            private BasicNeeds basicNeeds;
            private AudioClip noiseClip;
            private AudioSource noiseSource;
            private AudioSource playerSong;
            private bool wasDamaged = false;
            private bool coroutineStarted = false;
            private PlayerCollisionChecked playerCollisionChecked;
            private readonly List<AudioClip> vulnerableCollisionClips = new List<AudioClip>();
            public void Init(BasicNeeds bn, PlayerCollisionChecked pcc)
            {
                basicNeeds = bn;
                noiseClip = Resources.Load<AudioClip>("Health\\Shum_Low_Health");

                noiseSource = bn.gameObject.AddComponent<AudioSource>();
                playerSong = bn.gameObject.AddComponent<AudioSource>();
                playerSong.priority = 127;

                basicNeeds.HealthChangeValue += ChangeHealth;
                noiseSource.clip = noiseClip;
                noiseSource.volume = 0;
                noiseSource.loop = true;
                ChangeHealth(basicNeeds.Health);
                noiseSource.Play();

                playerCollisionChecked = pcc;
                playerCollisionChecked.PlayerTakingDamageEvent += OnPlayerVulnerableCollision;
                vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\ablat"));
                vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\bolnovnoge"));
                vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\shivi_shive"));
                vulnerableCollisionClips.Add(Resources.Load<AudioClip>("Health\\hma_hma"));
            }
            private void OnDisable()
            {
                basicNeeds.HealthChangeValue -= ChangeHealth;
                playerCollisionChecked.PlayerTakingDamageEvent -= OnPlayerVulnerableCollision;
            }
            /// <summary>
            /// вызов при столкновении, падении игрока (с игроком)
            /// </summary>
            private void OnPlayerVulnerableCollision()
            {
                AudioClip CalculateAudio()
                {
                    //цикл выполняется покуда не найдёт трек, который не является текущим
                    int index;
                    do
                    {
                        index = Random.Range(0, vulnerableCollisionClips.Count);
                    }
                    while (vulnerableCollisionClips[index] == playerSong.clip);
                    return vulnerableCollisionClips[index];
                }
                playerSong.Stop();
                playerSong.PlayOneShot(CalculateAudio());
            }
            private void ChangeHealth(float v)
            {
                //хп больше минимального выход
                if (v > minHealthForNoise)
                {
                    if (wasDamaged && !coroutineStarted)
                    {
                        StartCoroutine(nameof(ClipSilencer));
                        coroutineStarted = true;
                        wasDamaged = false;
                    }
                    return;
                }
                noiseSource.volume = minHealthForNoise / (v * 50);
                wasDamaged = true;
            }
            private IEnumerator ClipSilencer()
            {
                while (true)
                {
                    noiseSource.volume = Mathf.Lerp(noiseSource.volume, 0.001f, Time.deltaTime * 0.1f);
                    if (noiseSource.volume < 0.005f)
                    {
                        StopCoroutine(nameof(ClipSilencer));
                        coroutineStarted = false;
                        noiseSource.volume = 0;
                    }
                    yield return null;
                }
            }
        }
    }
}