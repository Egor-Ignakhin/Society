using UnityEngine;

public class SMGCamera : MonoBehaviour
{
    [SerializeField]
    private Transform activeGun;
    private Vector3 oldPos;

    private Quaternion activeGunDefRot;
    private void Awake()
    {
        activeGunDefRot = activeGun.rotation;
    }
    private void OnEnable()
    {
        ResetGunRotation();
    }
    public void RotateAroundMouse()
    {
        if (Input.GetMouseButton(0))
        {
            if (oldPos != Input.mousePosition)
            {
                float x = oldPos.x - Input.mousePosition.x;

                Vector3 target = new Vector3(0, x, 0);

                activeGun.localEulerAngles += target;
            }
        }
        oldPos = Input.mousePosition;
    }
    /// <summary>
    /// очищает ротацию оружия
    /// </summary>
    private void ResetGunRotation()
    {
        activeGun.rotation = activeGunDefRot;
    }
}
