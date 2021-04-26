using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    private bool wasKilled;
    [SerializeField] private Enemy mParent;
    private void Awake()
    {
        mParent.DeathEvent += Death;
    }
    private void Death()
    {
        wasKilled = true;
    }
    public void InjureEnemy(float value)
    {
        if (!wasKilled)
            mParent.InjureEnemy(value);
    }
    private void OnDisable()
    {
        mParent.DeathEvent -= Death;
    }
}
