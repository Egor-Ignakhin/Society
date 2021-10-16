using System.Collections;

using UnityEngine;

namespace Society.Effects
{
    [RequireComponent(typeof(AudioSource))]
    internal sealed class MusicPlayer : MonoBehaviour
    {
        [SerializeField] private System.Collections.Generic.List<AudioClip> loopedClips = new System.Collections.Generic.List<AudioClip>();
        [SerializeField] private AudioSource mAudioS;
        private int currentI = 0;
        private bool isStopped;
        private void Start()
        {
            StartCoroutine(nameof(IEnumeratorChangeClip));
        }
        private IEnumerator IEnumeratorChangeClip()
        {
            while (true)
            {
                if (isStopped)
                    break;
                ChangeClip();
                yield return new WaitForSeconds(mAudioS.clip.length);
            }
        }
        private void ChangeClip()
        {
            if (currentI == loopedClips.Count)
                currentI = 0;
            mAudioS.clip = loopedClips[currentI++];
            mAudioS.Play();
        }
        private void OnDestroy()
        {
            StopCoroutine(nameof(IEnumeratorChangeClip));
        }
        public void DisablePlayer()
        {
            isStopped = true;
            mAudioS.clip = null;
            StopCoroutine(nameof(IEnumeratorChangeClip));
        }
    }
}