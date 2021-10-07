using Society.Music;

using UnityEngine;
using UnityEngine.UI;

namespace Society.Menu
{
    sealed class MusicVolume : MonoBehaviour
    {
        private LocationMusic locationMusic;
        private void Start()
        {
            locationMusic = FindObjectOfType<LocationMusic>();
            GetComponent<Slider>().onValueChanged.AddListener(OnChangeValue);
        }
        private void OnChangeValue(float v)
        {
            locationMusic.SetVolume(v);
        }
        private void OnDestroy()
        {
            GetComponent<Slider>().onValueChanged.RemoveAllListeners();
        }
    }
}