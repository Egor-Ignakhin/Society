using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GunAnimator : MonoBehaviour
{
    [SerializeField] private Gun pistol;
    [SerializeField] private Transform aimPlace;
    [SerializeField] private Transform hangPlace;
    private FirstPersonController player;
    private float recoilM = 0.125f;

    private bool isAiming;// прицеливается ли игрок
    [ReadOnlyField] private bool isAnimFinish;
    [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.5f;
    private Camera playerCamera;
    private AdvancedSettings advanced = new AdvancedSettings();
    private void Awake()
    {
        playerCamera = Camera.main;
        advanced.baseCamFOV = playerCamera.fieldOfView;
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
    private void RecoilReceiver()
    {
        float value = (float)(lastAngle * Math.PI / 180);
        float cos = (float)Math.Abs(value);
        float sin = (float)Math.Cos(value);
        lastAngle += 12 * 1/recoilM;
        if (lastAngle > 360)
            lastAngle = 0;
        player.Recoil(new Vector3(cos, sin, 0) * recoilM);
    }
    public class AdvancedSettings
    {
        public float baseCamFOV;
        public float FOVKickAmount = 7.5f;
        public float fovRef;
    }
    private void Animate()
    {
        Vector3 target = isAiming ? aimPlace.position : hangPlace.position;
        pistol.transform.position = Vector3.MoveTowards(pistol.transform.position, target, lerpSpeed);

        isAnimFinish = pistol.transform.position == target;

        playerCamera.fieldOfView = Mathf.SmoothDamp(playerCamera.fieldOfView, pistol.transform.position == aimPlace.position ?
            advanced.baseCamFOV - (advanced.FOVKickAmount * 2) : advanced.baseCamFOV, ref advanced.fovRef, lerpSpeed);
    }
}
