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
        }
        public void SetEnableSimpleDOF(bool active) => volumeDOF.active = active;

        public void SetEnableBloom(bool v) => volumeBloom.active = v;
        internal void SetEnableAllEffects(bool v)
        {
            globalVolume.enabled = v;
        }
    }
}