using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPool : MonoBehaviour
{
    private readonly Stack<PoolableObject> reusableInstances = new Stack<PoolableObject>();

    protected PoolableObject prefabAsset;
    private Transform containerReturnedObject;
    private delegate void Action();
    private event Action ReusableCoroutine;

    private System.Collections.IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            ReusableCoroutine?.Invoke();
        }
    }
    public abstract void SetPrefabAsset(PoolableObject instance);
    protected void Preload()
    {
        containerReturnedObject = new GameObject($"{GetType()}_PoolContainer").transform;        
        for (int i = 0; i < PreLoadedCount(); i++)
        {
            var newAsset = Instantiate(prefabAsset);
            newAsset.OnInit(this);
            ReturnToPool(newAsset);
        }
    }
    protected abstract int PreLoadedCount();
    protected virtual bool UnityScale()
    {
        return true;
    }
    public void ReturnToPool(PoolableObject instance, bool disableObject = true)
    {
        if (disableObject)
        {
            instance.gameObject.SetActive(false);
            instance.transform.SetParent(containerReturnedObject);
            instance.transform.localPosition = Vector3.zero;
            if (UnityScale())
                instance.transform.localScale = Vector3.one;
            instance.transform.localEulerAngles = Vector3.one;
        }

        reusableInstances.Push(instance);
        ReusableCoroutine -= instance.DifferenceLifeTime;
    }
    protected PoolableObject GetObjectFromPool()
    {
        PoolableObject retObject;
        if (reusableInstances.Count > 0)
        {
            retObject = reusableInstances.Pop();
            retObject.gameObject.SetActive(true);
            retObject.transform.SetParent(null);
        }
        else
            retObject = Instantiate(prefabAsset);

        retObject.OnInit(this);
        ReusableCoroutine += retObject.DifferenceLifeTime;
        return retObject;
    }
}
