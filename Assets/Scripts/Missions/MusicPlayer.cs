using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private System.Collections.Generic.List<AudioClip> loopedClips = new System.Collections.Generic.List<AudioClip>();
    private AudioSource mAudioS;
    private int currentI = 0;
    private void Awake()
    {
        mAudioS = GetComponent<AudioSource>();
        StartCoroutine(nameof(IEnumeratorChangeClip));
    }    
    private IEnumerator IEnumeratorChangeClip()
    {
        while (true)
        {
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
}
