using UnityEngine;

public class FadeInOutParticles : MonoBehaviour
{

    private EffectSettings effectSettings;
    private ParticleSystem[] particles;
    private bool oldVisibleStat;

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
        particles = effectSettings.GetComponentsInChildren<ParticleSystem>();
        oldVisibleStat = effectSettings.IsVisible;
    }

    private void Update()
    {
        if (effectSettings.IsVisible != oldVisibleStat)
        {
            if (effectSettings.IsVisible)
                foreach (var particle in particles)
                {
                    if (effectSettings.IsVisible)
                    {
                        particle.Play();
                        var pe = particle.emission;
                        pe.enabled = true;
                    }
                }
            else
                foreach (var particle in particles)
                {
                    if (!effectSettings.IsVisible)
                    {
                        particle.Stop();
                        var pe = particle.emission;
                        pe.enabled = false;
                    }
                }
        }
        oldVisibleStat = effectSettings.IsVisible;
    }

}
