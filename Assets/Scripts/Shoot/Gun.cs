using UnityEngine;

/// <summary>
/// оружие
/// </summary>
public abstract class Gun : MonoBehaviour
{
    public delegate void RecoilHandler();
    public event RecoilHandler RecoilEvent;// событие перезарядки
    [SerializeField] protected int ammoCount;// количество патрон
    [SerializeField] protected float caliber = 10;// калибр снаряда    
    protected bool possibleShoot;// возможность стрелять
    public virtual float CartridgeDispenser() => 1;// возможная частота нажатия на курок в секунду
    private float currentCartridgeDispenser;

    public virtual float ReloadTime() => 5;// время перезарядки
    private float currentReloadTime = 0;
    private bool isReloaded = true;// перезаряжено ли оружие
    private Dispenser dispenser = new Dispenser(8, 8);
    [SerializeField] private Animator mAnimator;
    public bool isReload { get; protected set; }
    protected virtual void Awake()
    {
        mAnimator = GetComponent<Animator>();
    }
    protected virtual bool Shoot()
    {
        bool canShooting = ammoCount > 0 && currentCartridgeDispenser >= CartridgeDispenser() && dispenser.CountBullets > 0;
        if (canShooting)
        {
            ammoCount--;
            currentCartridgeDispenser = 0;
            dispenser.Dispens();
            mAnimator.SetTrigger("Fire");
        }
        else if (dispenser.CountBullets == 0)
        {
            isReloaded = false;
        }     
        return canShooting;
    }
    protected void CallRecoilEvent()
    {
        RecoilEvent?.Invoke();
    }
    protected void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Shoot();

        CartridgeDispens();

        if (!isReloaded)
        {
            Reload();
        }
    }
    private void CartridgeDispens()
    {
        if (currentCartridgeDispenser < CartridgeDispenser())
            currentCartridgeDispenser += Time.deltaTime;
    }
    private void Reload()
    {        
        if (currentReloadTime < ReloadTime())
        {
            currentReloadTime += Time.deltaTime;
            mAnimator.SetBool("Reload", true);
            isReload = true;
        }
        else
        {
            isReloaded = true;
            dispenser.Reload();
            currentReloadTime = 0;
            mAnimator.SetBool("Reload", false);
            isReload = false;
        }
    }
    public static float GetOptimalDamage(float G, float V, float F, float S,
        float distance, float maxDistance)
    {
        //mass * speed * area * shape coefficient
        float damage = 0.178f * G * V * F * S;

        if (distance != 0)
            damage /= (distance * 10 / maxDistance);
        Debug.Log(damage);
        return damage;
    }


    internal void SetPossibleShooting(bool isAnimFinish)
    {
        possibleShoot = isAnimFinish;
    }
    /// <summary>
    /// "магазин" оружия
    /// </summary>
    class Dispenser
    {
        public int CountBullets { get; private set; }
        private int maxBullets;
        public Dispenser(int cb, int maxB)
        {
            CountBullets = cb;
            maxBullets = maxB;
        }
        public void Dispens()
        {
            CountBullets--;
        }
        public void Reload()
        {
            CountBullets = maxBullets;
        }
    }
}

