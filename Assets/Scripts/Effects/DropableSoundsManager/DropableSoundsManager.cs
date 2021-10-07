using UnityEngine;
namespace Society.Effects.DropableSoundsManager
{
    public class DropableSoundsManager : MonoBehaviour
    {
        private AudioSource mAud;
        private AudioClip clips;
        private InvItemCollision currentCol;
        private void Awake()
        {
            transform.SetParent(null);
            mAud = GetComponent<AudioSource>();
            clips = Resources.Load<AudioClip>("DropSounds\\TT\\drop");
        }
        public void PlayClip(Vector3 pos, InvItemCollision col)
        {
            if (col == currentCol)
                return;
            mAud.Stop();
            currentCol = col;
            transform.position = pos;
            mAud.clip = clips;
            mAud.Play();
        }
        private void FixedUpdate()
        {
            if (!mAud.isPlaying)
                currentCol = null;
        }
    }
}