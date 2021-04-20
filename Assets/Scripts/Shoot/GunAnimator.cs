using UnityEngine;
using System;
using System.Collections.Generic;

namespace Shoots
{
    /// <summary>
    /// аниматор оружия
    /// </summary>
    class GunAnimator : Singleton<GunAnimator>
    {
        [SerializeField] private Gun pistol;
        [SerializeField] private Transform aimPlace;
        [SerializeField] private Transform hangPlace;
        private FirstPersonController fps;
        public bool isAiming { get; private set; }// прицеливается ли игрок
        [ReadOnlyField] private bool isAnimFinish;
        [SerializeField] [Range(0, 1)] private float lerpSpeed = 0.02f;// скорость смены состояний : 1) у бедра 2) прицельный огонь
        [SerializeField] private List<Camera> cameras = new List<Camera>();
        private AdvancedSettings advanced;
        private enum States { dSlant, LSlant, Rlant };

        private void Awake()
        {
            advanced = new AdvancedSettings(cameras[0].fieldOfView);
            pistol.RecoilEvent += RecoilReceiver;
            fps = FindObjectOfType<FirstPersonController>();
        }
        private void Update()
        {
            isAiming = Input.GetMouseButton(1);

            TiltCamera(GetSlant());


            Animate();
            pistol.SetPossibleShooting(isAnimFinish);
        }
        private States GetSlant()
        {
            if (Input.GetKey(KeyCode.Q))
            {
                return States.LSlant;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                return States.Rlant;
            }

            return States.dSlant;
        }
        private void TiltCamera(States s)
        {
            if (s == States.LSlant && isAiming)
            {
                fps.SetZSlant(15);
            }
            else if (s == States.Rlant && isAiming)
            {
                fps.SetZSlant(-15);
            }
            else
            {
                fps.SetZSlant(0);
            }
        }
        private double lastAngle = 0;

        /// <summary>
        /// рисовщик отдачи. По умолчанию рисует кривую косинуса
        /// </summary>
        private void RecoilReceiver()
        {
            double value = (lastAngle += 12 / pistol.getRecoilPower()) * Math.PI / 180;
            float cos = (float)Math.Abs(Math.Sin(value));
            float sin = (float)Math.Cos(value);

            fps.Recoil(new Vector3(cos, sin, 0));
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
            fps.SensivityM = isAiming ? 0.25f : 1;

            float targetFOV = isAiming && !pistol.IsReload ?// анимирование угла обзора
                  advanced.BaseCamFOV - (advanced.FOVKickAmount * 3) : advanced.BaseCamFOV;
            foreach (var c in cameras)
            {
                c.fieldOfView = Mathf.SmoothDamp(c.fieldOfView, targetFOV, ref advanced.fovRef, lerpSpeed);
            }
            if (pistol.IsReload)
                return;
            Quaternion q = isAiming && !pistol.IsReload ? aimPlace.rotation : hangPlace.rotation;// следующая позиция
            pistol.transform.rotation = Quaternion.RotateTowards(pistol.transform.rotation, q, 1);
        }
    }
}