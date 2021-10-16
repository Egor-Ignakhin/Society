using UnityEngine;

public class DebuffOnEnemyFromCollision : MonoBehaviour
{

    public EffectSettings EffectSettings;
    public GameObject Effect;

    // Use this for initialization
    private void Start()
    {
        EffectSettings.CollisionEnter += EffectSettings_CollisionEnter;
    }

    private void EffectSettings_CollisionEnter(object sender, CollisionInfo e)
    {
        if (Effect == null)
            return;
        var colliders = Physics.OverlapSphere(transform.position, EffectSettings.EffectRadius, EffectSettings.LayerMask);
        foreach (var coll in colliders)
        {
            var hitGO = coll.transform;
            var renderer = hitGO.GetComponentInChildren<Renderer>();
            var effectInstance = Instantiate(Effect);
            effectInstance.transform.parent = renderer.transform;
            effectInstance.transform.localPosition = Vector3.zero;
            effectInstance.GetComponent<AddMaterialOnHit>().UpdateMaterial(coll.transform);
        }
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
