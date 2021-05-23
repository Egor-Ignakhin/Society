using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private readonly Stack<PoolableObject> reusableInstances = new Stack<PoolableObject>();

    protected PoolableObject prefabAsset;
    protected void ReturnToPool(PoolableObject instance)
    {
        instance.gameObject.SetActive(false);
        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        instance.transform.localEulerAngles = Vector3.one;

        reusableInstances.Push(instance);
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

        return retObject;
    }
}
