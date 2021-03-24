using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private float health;
    public float Health
    {
        get { return health; }
        protected set
        {
            if (value <= minHealth)
                Death();
            health = value;
        }
    }

    private float minHealth = 0;
    protected BasicNeeds currentEnemy;
    public BasicNeeds CurrentEnemy
    {
        get => currentEnemy;
        protected set
        {
            currentEnemy = value;
        }
    }

    protected abstract void Attack();
    protected abstract void HarassmentEnemy();
    public abstract void InjureEnemy(float value);
    protected abstract void Death();

}
