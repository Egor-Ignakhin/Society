using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int Health { get; protected set; }
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
    public abstract void InjureEnemy();
    protected abstract void Death();

}
