using UnityEngine;

public sealed class MedicalDoor : MonoBehaviour
{
    private Animator mAnimator;
    //private AudioSource mAudioSource;
    [SerializeField] private AudioClip openCloseClip;
    private void Awake()
    {
        mAnimator = GetComponent<Animator>();
        //    mAudioSource = GetComponent<AudioSource>();
    }
    internal void OnEnterPlayer()
    {
        mAnimator.CrossFade("Open", 1);
        //mAudioSource.pitch = 1;
        //mAudioSource.PlayOneShot(openCloseClip);
    }

    internal void OnExitPlayer()
    {
        mAnimator.CrossFade("Close", 1);
        //mAudioSource.pitch = -1;
        //mAudioSource.PlayOneShot(openCloseClip);
    }
}
