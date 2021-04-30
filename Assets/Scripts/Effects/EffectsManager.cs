using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsManager : Singleton<EffectsManager>
{
    private Volume globalVolume;
    private DepthOfField volumeDOF;
    private Bloom volumeBloom;

    public override void Init()
    {
        globalVolume = FindObjectOfType<Volume>();
        if (!globalVolume)
            return;
        globalVolume.profile.TryGet(out volumeDOF);
        globalVolume.profile.TryGet(out volumeBloom);

        SetEnableDOF(false);
        base.Init();
    }
    /// <summary>
    /// set enable depth of field
    /// </summary>
    public void SetEnableDOF(bool active)
    {           
        //dph.focusDistance.value = 69;
        //dph.aperture.value = 30;
        volumeDOF.active = active;
        //dph.kernelSize.value = KernelSize.VeryLarge;
    }    
    public void SetEnableBloom(bool v)
    {
        volumeBloom.active = v;
    }
}
