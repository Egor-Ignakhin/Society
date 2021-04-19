using System.Collections;
using UnityEngine;

public class UsedUpBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime = 5.0f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
