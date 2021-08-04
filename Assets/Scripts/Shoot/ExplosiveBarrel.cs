using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IBulletReceiver
{
 
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject barrel;
    [Range(1, 100)] public float damageRadius;
    [Range(1, 1000)] public float maxDamage;
    private float damageGradient;

    public void OnBulletEnter()
    {
        explosion.SetActive(true);
        barrel.SetActive(false);
        DamageEnemies();
    }
 
    void Start()
    {
        explosion.SetActive(false);
        damageGradient = maxDamage / damageRadius;
    }

    void DamageEnemies()
    {
        Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, damageRadius);
        Debug.Log(collidersInRadius.Length);
        foreach (Collider col in collidersInRadius)
        {
            EnemyCollision obj = col.gameObject.GetComponent<EnemyCollision>();

            if (obj != null)
            {
                DamageEnemy(obj);
            }
        }
    }

    void DamageEnemy(EnemyCollision enemy)
    {

        float dist = (transform.position - enemy.transform.position).magnitude;
        //Крайне упрощенная модель зависимости повреждений от дистанции
        float damage = damageGradient * dist;
        enemy.InjureEnemy(damage);
    }
    /*
    void Update()
    {
        DamageEnemies();
    }
    */

}
