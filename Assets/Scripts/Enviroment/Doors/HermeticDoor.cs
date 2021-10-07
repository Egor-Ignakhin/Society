using System;
using System.Threading.Tasks;

using UnityEngine;

namespace Society.Enviroment.Doors
{
    public sealed class HermeticDoor : MonoBehaviour
    {
        private AudioSource mAud;
        private bool isOpening = true;

        private AudioClip OpeningClip;
        private AudioClip ClosingClip;

        private void Start()
        {
            mAud = GetComponent<AudioSource>();
            OpeningClip = Resources.Load<AudioClip>("DoorClips\\HermeticDoor\\HermeticDoor_Open");
            ClosingClip = Resources.Load<AudioClip>("DoorClips\\HermeticDoor\\HermeticDoor_Close");
        }
        public async void Interact(bool isOpening, Action callMth)
        {
            this.isOpening = isOpening;
            var clip = GetClipByState();
            mAud.PlayOneShot(clip);
            await Task.Delay((int)(clip.length * 1000));
            callMth.Invoke();
        }

        private AudioClip GetClipByState()
        {
            return isOpening ? OpeningClip : ClosingClip;
        }
    }
}