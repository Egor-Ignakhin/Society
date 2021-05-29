using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// класс отвечающий за пост-обработку
/// </summary>
public sealed class EffectsManager : Singleton<EffectsManager>
{
    private Volume globalVolume;
    private DepthOfField volumeDOF;
    private Bloom volumeBloom;
    private MotionBlur motionBlur;
    private bool IsRechargeable = false;//перезаряжается ли оружие
    private EffectsSettings effectsSettings = new EffectsSettings();
    public class EffectsSettings
    {
        public float rechargeableBlurIntensity = 0.5f;
    }
    public override void Init()
    {
        globalVolume = FindObjectsOfType<Volume>().Where(v => v.isGlobal).First();
        if (!globalVolume)
            return;
        globalVolume.profile.TryGet(out volumeDOF);
        globalVolume.profile.TryGet(out volumeBloom);
        globalVolume.profile.TryGet(out motionBlur);

        base.Init();        
    }
    /// <summary>
    /// set enable depth of field
    /// </summary>
    public void SetEnableDOF(bool active) => volumeDOF.active = active;

    /// <summary>
    /// set enable bloom
    /// </summary>
    /// <param name="v"></param>
    public void SetEnableBloom(bool v) => volumeBloom.active = v;


    private void Update() => AnimateMotionBlur();

    /// <summary>
    /// анимируется при перезарядке любого оружия
    /// </summary>
    private void AnimateMotionBlur()
    {
        //    motionBlur.active = true;
        // if (IsRechargeable)
        //   motionBlur.intensity = new ClampedFloatParameter(effectsSettings.rechargeableBlurIntensity, 0, 1,true);
        //    else
        //      motionBlur.intensity = new ClampedFloatParameter(0, 0, 1);
    }

    internal void SetActiveBlur(bool IRe)
    {
        //   IsRechargeable = IRe;
    }

    internal void SetEnableAllEffects(bool v)
    {
        globalVolume.enabled = v;
    }
}
