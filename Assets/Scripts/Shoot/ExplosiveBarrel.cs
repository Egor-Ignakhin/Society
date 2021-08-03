using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IBulletReceiver
{
 
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject barrel;



    public void OnBulletEnter()
    {
        explosion.SetActive(true);
        barrel.SetActive(false);
    }
 
    void Start()
    {
        explosion.SetActive(false);
    }

    void Update()
    {



    }
}
