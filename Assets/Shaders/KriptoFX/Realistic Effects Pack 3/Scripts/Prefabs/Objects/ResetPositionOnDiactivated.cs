using UnityEngine;

public class ResetPositionOnDiactivated : MonoBehaviour
{

    public EffectSettings EffectSettings;

    private void Start()
    {
        EffectSettings.EffectDeactivated += EffectSettings_EffectDeactivated;
    }

    private void EffectSettings_EffectDeactivated(object sender, System.EventArgs e)
    {
        transform.localPosition = Vector3.zero;
    }
}
