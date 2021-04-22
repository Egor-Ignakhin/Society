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
        private readonly List<GameObject> gunsContainers = new List<GameObject>();
        private readonly List<PlayerGun> guns = new List<PlayerGun>();// список для оружия, и их точек стрельбы; переноса
        public class PlayerGun
        {
            public Gun mGun { get; }
            public Transform HangPlace { get; }
            public Transform AimPlace { get; }
            public PlayerGun(Gun g, Transform hp, Transform ap)
            {
                mGun = g;
                HangPlace = hp;
                AimPlace = ap;
            }
        }

        private FirstPersonController fps;
        public bool isAiming { get; private set; }// прицеливается ли игрок
        [ReadOnlyField] private bool isAnimFinish;
        [SerializeField] [Range(0, 1)] private float lerpSpeed = 10;// скорость смены состояний : 1) у бедра 2) прицельный огонь
        [SerializeField] private List<Camera> cameras = new List<Camera>();
        private AdvancedSettings advanced;
        private enum States { dSlant, LSlant, Rlant };

        private int currentI = 0;

        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
                gunsContainers.Add(transform.GetChild(i).gameObject);

            for (int i = 0; i < gunsContainers.Count; i++)
            {
                Gun gun = null;
                Transform hangPlace = null;
                Transform aimPlace = null;
                for (int k = 0; k < gunsContainers[i].transform.childCount; k++)
                {
                    var cg = gunsContainers[i].transform.GetChild(k);
                    if (cg.name == "HangPlace")
                    {
                        hangPlace = cg.transform;
                    }
                    else if (cg.name == "AimPlace")
                    {
                        aimPlace = cg.transform;
                    }
                    else
                        gun = cg.GetComponent<Gun>();
                }
                guns.Add(new PlayerGun(gun, hangPlace, aimPlace));
            }

            TestChangeGun(0);
            advanced = new AdvancedSettings(cameras[0].fieldOfView);
            guns[currentI].mGun.RecoilEvent += RecoilReceiver;
            fps = FindObjectOfType<FirstPersonController>();
        }
        private void Update()
        {
            isAiming = Input.GetMouseButton(1);

            TiltCamera(GetSlant());

            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0)
                TestChangeGun(scroll);

            Animate();
            guns[currentI].mGun.SetPossibleShooting(isAnimFinish);
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

        public void TestChangeGun(float scroll)
        {
            guns[currentI].mGun.RecoilEvent -= RecoilReceiver;
            if (scroll > 0)
                currentI++;
            else if (scroll < 0)
                currentI--;
            if (currentI < 0)
                currentI = guns.Count - 1;
            else if (currentI > guns.Count - 1)
                currentI = 0;

            guns[currentI].mGun.RecoilEvent += RecoilReceiver;
            DisableGuns();
        }

        private void DisableGuns()
        {
            for (int i = 0; i < guns.Count; i++)
            {
                gunsContainers[i].SetActive(i == currentI);
            }
        }
        private double lastAngle = 0;

        /// <summary>
        /// рисовщик отдачи. По умолчанию рисует кривую косинуса
        /// </summary>
        private void RecoilReceiver()
        {
            double value = (lastAngle += 12 / guns[currentI].mGun.getRecoilPower()) * Math.PI / 180;
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
            var gun = guns[currentI].mGun;
            var tGun = gun.transform;
            var aimPlace = guns[currentI].AimPlace;
            var hangPlace = guns[currentI].HangPlace;

            Vector3 target = isAiming && !gun.IsReload ? aimPlace.position : hangPlace.position;// следующая позиция
            tGun.position = Vector3.MoveTowards(gun.transform.position, target, Time.deltaTime / lerpSpeed);

            isAnimFinish = tGun.position == target;
            fps.SensivityM = isAiming ? 0.25f : 1;

            float targetFOV = isAiming && !gun.IsReload ?// анимирование угла обзора
                  advanced.BaseCamFOV - (advanced.FOVKickAmount * 3) : advanced.BaseCamFOV;
            foreach (var c in cameras)
            {
                c.fieldOfView = Mathf.SmoothDamp(c.fieldOfView, targetFOV, ref advanced.fovRef, lerpSpeed);
            }
            if (gun.IsReload)
                return;
            Quaternion q = isAiming && !gun.IsReload ? aimPlace.rotation : hangPlace.rotation;// следующая позиция
            tGun.rotation = Quaternion.RotateTowards(gun.transform.rotation, q, 1);
        }
    }
}