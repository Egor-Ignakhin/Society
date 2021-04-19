using UnityEngine;
using System;
/// <summary>
/// аниматор оружия
/// </summary>
class GunAnimator : MonoBehaviour
{
    [SerializeField] private Gun pistol;
    [SerializeField] private Transform aimPlace;
    [SerializeField] private Transform hangPlace;
    private FirstPersonController player;
    private float recoilM = 0.125f;

    private bool isAiming;// прицеливается ли игрок
    [ReadOnlyField] private bool isAnimFinish;
    [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.02f;// скорость смены состояний : 1) у бедра 2) прицельный огонь
    private Camera playerCamera;
    private AdvancedSettings advanced;
    private void Awake()
    {
        playerCamera = Camera.main;
        advanced = new AdvancedSettings(playerCamera.fieldOfView);
        pistol.RecoilEvent += RecoilReceiver;
        player = FindObjectOfType<FirstPersonController>();
    }
    private void Update()
    {
        isAiming = Input.GetMouseButton(1);
        Animate();
        pistol.SetPossibleShooting(isAnimFinish);
    }
    private float lastAngle = 0;

    /// <summary>
    /// рисовщик отдачи. По умолчанию рисует кривую косинуса
    /// </summary>
    private void RecoilReceiver()
    {
        float value = (float)(lastAngle * Math.PI / 180);
        float cos = (float)Math.Abs(value);
        float sin = (float)Math.Cos(value);
        lastAngle += 12 * 1 / recoilM;
        if (lastAngle > 360)
            lastAngle = 0;
        player.Recoil(new Vector3(cos, sin, 0) * recoilM);
    }
    /// <summary>
    /// настройки анимаций
    /// </summary>
    public class AdvancedSettings
    {
        public float BaseCamFOV { get; }
        public float FOVKickAmount { get; } = 7.5f;
        public float fovRef;
        public AdvancedSettings(float bcf)
        {
            BaseCamFOV = bcf;
        }
    }
    private void Animate()
    {
        Vector3 target = isAiming && !pistol.isReload ? aimPlace.position : hangPlace.position;// следующая позиция
        pistol.transform.position = Vector3.MoveTowards(pistol.transform.position, target, lerpSpeed);


        isAnimFinish = pistol.transform.position == target;

        playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, pistol.transform.position == aimPlace.position ?// анимирование угла обзора
            advanced.BaseCamFOV - (advanced.FOVKickAmount * 2) : advanced.BaseCamFOV, ref advanced.fovRef, lerpSpeed);
        if (pistol.isReload)
            return;
        pistol.transform.localRotation = Quaternion.RotateTowards(pistol.transform.localRotation, Quaternion.identity, 1);     
    }
}
