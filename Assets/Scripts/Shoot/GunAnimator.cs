﻿using System;
using System.Collections.Generic;

using Society.Inventory;
using Society.Player.Controllers;
using Society.Settings;

using UnityEngine;

namespace Society.Shoot
{
    /// <summary>
    /// аниматор оружия
    /// </summary>
    internal class GunAnimator : MonoBehaviour
    {
        /// <summary>
        /// Пустышки содержащие <see cref="Gun"/> и Hang/Aim places
        /// </summary>
        [SerializeField] private List<GameObject> gunContainers = new List<GameObject>();
        [SerializeField] private List<PlayerGun> guns = new List<PlayerGun>();// список для оружия, и их точек стрельбы; переноса
        [Serializable]
        public class PlayerGun
        {
            public Gun MGun;
            public Transform HangPlace;
            public Transform AimPlace;
        }

        private FirstPersonController fps;
        public bool IsAiming { get; private set; }// прицеливается ли игрок
        [ReadOnlyField] private bool isAnimFinish;
        [SerializeField] [Range(0, 1)] private float lerpSpeed = 10;// скорость смены состояний : 1) у бедра 2) прицельный огонь
        [SerializeField] private Camera mainCamera;
        private AdvancedSettings advanced;
        private enum States { dSlant, LSlant, Rlant };

        private int currentActiveGunI = -1;

        [SerializeField] private LayerMask interactableLayers;
        private AudioSource unitAudioSource;
        private InventoryContainer inventoryContainer;
        private InventoryEventReceiver InventoryEventReceiver;
        private SMG.SMGEventReceiver SMGEventReceiver;
        private SMG.SMGMain SMGMain;

        private void Start()
        {
            DisableGuns();
            advanced = new AdvancedSettings();

            fps = FindObjectOfType<FirstPersonController>();
            unitAudioSource = GetComponent<AudioSource>();

            InventoryEventReceiver.ChangeSelectedCellEvent += ChangeGun;

            foreach (var g in guns)
            {
                g.MGun.OnInit(interactableLayers, this);
            }
            inventoryContainer = FindObjectOfType<InventoryContainer>();
            InventoryEventReceiver = inventoryContainer.EventReceiver;
            SMGMain = FindObjectOfType<SMG.SMGMain>();
            SMGEventReceiver = SMGMain.EventReceiver;
        }
        private void OnDisable()
        {
            gunContainers = null;
            guns = null;
            advanced = null;
            fps = null;
            InventoryEventReceiver.ChangeSelectedCellEvent -= ChangeGun;
        }
        private void Update()
        {
            IsAiming = Input.GetMouseButton(1) && (currentActiveGunI != -1);

            TiltCamera(GetSlant());

            Animate();
            if (currentActiveGunI != -1)
                guns[currentActiveGunI].MGun.SetPossibleShooting(isAnimFinish && !SMGMain.IsActive);
        }

        private States GetSlant()
        {
            if (Input.GetKey(GameSettings.GetLeanLeftKeyCode()))
                return States.LSlant;
            else if (Input.GetKey(GameSettings.GetLeanRightKeyCode()))
                return States.Rlant;

            return States.dSlant;
        }
        private void TiltCamera(States s)
        {
            if (s == States.LSlant && IsAiming)
                fps.SetZSlant(10);

            else if (s == States.Rlant && IsAiming)
                fps.SetZSlant(-10);

            else
                fps.SetZSlant(0);
        }

        public void ChangeGun(int id)
        {
            if (currentActiveGunI != -1)
            {
                guns[currentActiveGunI].MGun.ShootEvent -= RecoilReceiver;
                if (InventoryEventReceiver.GetLastSelectedCell())
                {
                    guns[currentActiveGunI].MGun.ChangeAmmoCountEvent -= InventoryEventReceiver.GetLastSelectedCell().SetAmmoCount;
                    SMGEventReceiver.UpdateModfiersEvent -= UpdateGunModifiers;
                }
            }

            switch (id)
            {
                case (int)ItemStates.ItemsID.TTPistol:
                    currentActiveGunI = 0;
                    break;
                case (int)ItemStates.ItemsID.Ak_74:
                    currentActiveGunI = 1;
                    break;
                default:
                    currentActiveGunI = -1;
                    break;
            }
            DisableGuns();
            if (currentActiveGunI == -1)
                return;
            guns[currentActiveGunI].MGun.ShootEvent += RecoilReceiver;
            var sc = InventoryEventReceiver.GetSelectedCell();
            guns[currentActiveGunI].MGun.ChangeAmmoCountEvent += sc.SetAmmoCount;
            SMGEventReceiver.UpdateModfiersEvent += UpdateGunModifiers;
            guns[currentActiveGunI].MGun.UpdateModifiers(sc.MGun.Mag, sc.MGun.Aim, sc.MGun.Silencer);
        }

        public void UpdateGunModifiers(Society.SMG.ModifierCell _)
        {
            var ic = InventoryEventReceiver.GetSelectedCell();
            if (!ic)
                return;

            //если в оружие больше 0 патрон
            if (ic.MGun.AmmoCount > 0)
                inventoryContainer.AddItem((int)guns[currentActiveGunI].MGun.GetBulletId(), ic.MGun.AmmoCount, null);// добавление кол-ва из магазина

            guns[currentActiveGunI].MGun.UpdateModifiers(ic.MGun.Mag, ic.MGun.Aim, ic.MGun.Silencer);
            ic.SetAmmoCount(0);
        }
        private void DisableGuns()
        {
            for (int i = 0; i < guns.Count; i++)
            {
                gunContainers[i].SetActive(i == currentActiveGunI);
            }
        }
        private double lastAngle = 0;

        /// <summary>
        /// рисовщик отдачи. По умолчанию рисует кривую косинуса
        /// </summary>
        private void RecoilReceiver()
        {
            double value = (lastAngle += 12 / guns[currentActiveGunI].MGun.GetRecoilPower()) * Math.PI / 180;
            float cos = (float)Math.Abs(Math.Sin(value));
            float sin = (float)Math.Cos(value);

            fps.Rocking(new Vector3(cos, sin, 0));
        }
        /// <summary>
        /// настройки анимаций
        /// </summary>
        public class AdvancedSettings
        {
            public float BaseCamFOV => (float)Settings.GameSettings.GetFieldOfView();
            public const float FOVAim = 22.5f;
            public float fovRef;
        }
        private void Animate()
        {
            if (Society.GameScreens.ScreensManager.HasActiveScreen())
                return;
            if (currentActiveGunI == -1)// if item isn't gun
                return;
            var gun = guns[currentActiveGunI].MGun;
            var tGun = gun.transform;
            var aimPlace = guns[currentActiveGunI].AimPlace;
            var hangPlace = guns[currentActiveGunI].HangPlace;

            Vector3 target = IsAiming && !gun.IsReload ? aimPlace.position : hangPlace.position;// следующая позиция
            tGun.position = Vector3.MoveTowards(gun.transform.position, target, Time.deltaTime / lerpSpeed);

            isAnimFinish = tGun.position == target;
            fps.SensivityM = IsAiming ? 0.25f : 1;

            float targetFOV = IsAiming && !gun.IsReload ?// анимирование угла обзора
                  advanced.BaseCamFOV - (AdvancedSettings.FOVAim) : advanced.BaseCamFOV;

            mainCamera.fieldOfView = Mathf.SmoothDamp(mainCamera.fieldOfView, targetFOV, ref advanced.fovRef, lerpSpeed);

            if (gun.IsReload)
                return;
            Quaternion q = IsAiming && !gun.IsReload ? aimPlace.rotation : hangPlace.rotation;// следующая позиция
            tGun.rotation = Quaternion.RotateTowards(gun.transform.rotation, q, 1);
        }
        public void PlayArmorySound(AudioClip clip)
        {
            unitAudioSource.PlayOneShot(clip);
        }
    }
}