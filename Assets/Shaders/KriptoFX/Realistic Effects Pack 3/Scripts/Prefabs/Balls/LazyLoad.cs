using UnityEngine;

public class LazyLoad : MonoBehaviour
{

    public GameObject GO;
    public float TimeDelay = 0.3f;

    // Use this for initialization

    private void Awake()
    {
        GO.SetActive(false);
    }

    // Update is called once per frame
    private void LazyEnable()
    {
        GO.SetActive(true);
    }

    private void OnEnable()
    {
        Invoke("LazyEnable", TimeDelay);
    }

    private void OnDisable()
    {
        CancelInvoke("LazyEnable");
        GO.SetActive(false);
    }
}
