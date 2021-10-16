using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour
{

    public Animator anim;

    private EffectSettings effectSettings;
    private bool isInitialized;
    private float oldSpeed;

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

    // Use this for initialization
    private void Start()
    {
        oldSpeed = anim.speed;
        GetEffectSettingsComponent(transform);
        if (effectSettings != null)
            effectSettings.CollisionEnter += prefabSettings_CollisionEnter;

        isInitialized = true;
    }

    private void OnEnable()
    {
        if (isInitialized) anim.speed = oldSpeed;
    }

    private void prefabSettings_CollisionEnter(object sender, CollisionInfo e)
    {
        anim.speed = 0;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
