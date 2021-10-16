using UnityEngine;

public class OnRigidbodySendCollision : MonoBehaviour
{

    private EffectSettings effectSettings;

    private void GetEffectSettingsComponent(Transform tr)
    {
        var parent = tr.parent;
        if (parent != null)
        {
            effectSettings = parent.GetComponentInChildren<EffectSettings>();
            if (effectSettings == null)
                GetEffectSettingsComponent(parent.transform);
        }
    }

    private void Start()
    {
        GetEffectSettingsComponent(transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        effectSettings.OnCollisionHandler(new CollisionInfo());
    }
}
