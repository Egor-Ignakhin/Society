using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

namespace Society.Effects
{

    /// <summary>
    /// класс отвечающий за пост-обработку
    /// </summary>
    public sealed class EffectsManager : MonoBehaviour
    {
        private Volume globalVolume;
        private DepthOfField volumeDOF;
        private Bloom volumeBloom;
        public void Init()
        {
            globalVolume = GameObject.Find("Global Volume Real").GetComponent<Volume>();
            if (!globalVolume)
                return;

            globalVolume.profile.TryGet(out volumeDOF);
            globalVolume.profile.TryGet(out volumeBloom);

            Menu.Settings.SettingsManager.SettingsUpdateEvent += OnUpdateSettings;
        }
        public void SetEnableSimpleDOF(bool active) => volumeDOF.active = active;

        internal void SetEnableGlobalVolume(bool v)
        {
            globalVolume.enabled = v;
        }

        private void OnUpdateSettings()
        {

            if (volumeBloom)
                volumeBloom.active = Settings.VideoSettings.GetBloomIsEnabled();
        }
        private void OnDestroy()
        {
            Menu.Settings.SettingsManager.SettingsUpdateEvent -= OnUpdateSettings;
        }
    }
}