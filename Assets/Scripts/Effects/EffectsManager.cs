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
        volumeDOF.active = active;
    }    
    /// <summary>
    /// set enable bloom
    /// </summary>
    /// <param name="v"></param>
    public void SetEnableBloom(bool v)
    {
        volumeBloom.active = v;
    }
}
