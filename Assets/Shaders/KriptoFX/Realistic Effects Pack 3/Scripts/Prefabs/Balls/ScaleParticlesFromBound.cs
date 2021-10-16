using UnityEngine;

public class ScaleParticlesFromBound : MonoBehaviour
{

    private Collider targetCollider;

    private void GetMeshFilterParent(Transform t)
    {
        var coll = t.parent.GetComponent<Collider>();
        if (coll == null)
            GetMeshFilterParent(t.parent);
        else
            targetCollider = coll;
    }

    // Use this for initialization
    private void Start()
    {
        GetMeshFilterParent(transform);
        if (targetCollider == null) return;
        var boundSize = targetCollider.bounds.size;
        transform.localScale = boundSize;
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
