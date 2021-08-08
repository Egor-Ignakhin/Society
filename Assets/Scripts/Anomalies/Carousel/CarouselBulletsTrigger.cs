using System;
using UnityEngine;

class CarouselBulletsTrigger : MonoBehaviour, IBulletReceiver
{
    private bool needBlust;
    [SerializeField] private ParticleSystem aura;
    private float defaultStartSizeM;
    private void Awake()
    {
        defaultStartSizeM = aura.main.startSpeedMultiplier;
    }
    public void OnBulletEnter(Vector3 point, float force)
    {
        needBlust = true;
        ParticleSystem.MainModule main = aura.main;
        main.startSizeMultiplier += 2;
    }
    private void FixedUpdate()
    {
        if (needBlust)
            AnimateBlust();
    }

    private void AnimateBlust()
    {
        ParticleSystem.MainModule main = aura.main;
        if ((main.startSizeMultiplier -= (Time.fixedDeltaTime * 100)) <= defaultStartSizeM)
            needBlust = false;
    }
}
