using UnityEngine;
using System;
using System.Collections.Generic;

namespace Shoots
{
    /// <summary>
    /// аниматор оружия
    /// </summary>
    class GunAnimator : MonoBehaviour
    {
        [SerializeField] private Gun pistol;
        [SerializeField] private Transform aimPlace;
        [SerializeField] private Transform hangPlace;
        private FirstPersonController player;

        private bool isAiming;// прицеливается ли игрок
        [ReadOnlyField] private bool isAnimFinish;
        [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.02f;// скорость смены состояний : 1) у бедра 2) прицельный огонь
        [SerializeField] private List<Camera> cameras = new List<Camera>(); 
        private AdvancedSettings advanced;
        private void Awake()
        {
            advanced = new AdvancedSettings(cameras[0].fieldOfView);
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
            lastAngle += 12 * 1 / pistol.getRecoilPower();
            if (lastAngle > 360)
                lastAngle = 0;
            player.Recoil(new Vector3(cos, sin, 0) );
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
            Vector3 target = isAiming && !pistol.IsReload ? aimPlace.position : hangPlace.position;// следующая позиция
            pistol.transform.position = Vector3.MoveTowards(pistol.transform.position, target, lerpSpeed);

            isAnimFinish = pistol.transform.position == target;

            foreach (var c in cameras)
            {
             //   c.fieldOfView = Mathf.SmoothDamp(c.fieldOfView, pistol.transform.position == aimPlace.position ?// анимирование угла обзора
             //     advanced.BaseCamFOV - (advanced.FOVKickAmount * 2) : advanced.BaseCamFOV, ref advanced.fovRef, lerpSpeed);
            }
            if (pistol.IsReload)
                return;
            pistol.transform.localRotation = Quaternion.RotateTowards(pistol.transform.localRotation, Quaternion.identity, 1);
        }
    }
}