using UnityEngine;

/// <summary>
/// класс-контроллёр для компаса
/// </summary>
class CompassController : MonoBehaviour
{
    [SerializeField] private Maps.MapDrawer mapDrawer;
    private void OnEnable() => mapDrawer.RotateEvent += Rotate;

    private void Rotate(Vector3 currentRot) => transform.localRotation = Quaternion.Euler(currentRot);

    private void OnDisable() => mapDrawer.RotateEvent -= Rotate;

}
