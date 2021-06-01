using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// класс отвечающий за пост-обработку
/// </summary>
public sealed class EffectsManager : MonoBehaviour
{
    private Volume globalVolume;
    private DepthOfField volumeDOF;
    private Bloom volumeBloom;
    private ChromaticAberration chromaticAberration;
    private bool isRechargeable;
    private bool NeccecryAnimateCAB = true;
    public void Init()
    {
        globalVolume = FindObjectsOfType<Volume>().Where(v => v.isGlobal).First();
        if (!globalVolume)
            return;
        globalVolume.profile.TryGet(out volumeDOF);
        globalVolume.profile.TryGet(out volumeBloom);
        globalVolume.profile.TryGet(out chromaticAberration);
        chromaticAberration.active = NeccecryAnimateCAB;
    }
    public void SetEnableSimpleDOF(bool active) => volumeDOF.active = active;

    public void SetEnableBloom(bool v) => volumeBloom.active = v;
    private void Update() => AnimateChromaticAb();

    private void AnimateChromaticAb()
    {
        if (!NeccecryAnimateCAB)
            return;
        if (isRechargeable && chromaticAberration.intensity.value <= 0.25f)
            chromaticAberration.intensity.value += Time.deltaTime;
        else if (chromaticAberration.intensity.value > 0)
            chromaticAberration.intensity.value -= Time.deltaTime;
    }
    internal void SetEnableAllEffects(bool v) => globalVolume.enabled = v;
    public void SetRechargeable(bool v)
    {
        if (!NeccecryAnimateCAB)
            return;
        isRechargeable = v;
        chromaticAberration.active = true;
    }
}
