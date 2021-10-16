using UnityEngine;

public class RealtimeReflection : MonoBehaviour
{

#if UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6
#else
    private ReflectionProbe probe;
    private Transform camT;

    private void Awake()
    {
        probe = GetComponent<ReflectionProbe>();
        camT = Camera.main.transform;
    }

    private void Update()
    {
        var pos = camT.position;
        probe.transform.position = new Vector3(
            pos.x,
            pos.y * -1,
            pos.z
        );
        probe.RenderProbe();
    }
#endif
}
