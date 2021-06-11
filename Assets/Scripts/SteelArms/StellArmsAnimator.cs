using Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace S.A
{
    public class StellArmsAnimator : MonoBehaviour
    {
        private Dictionary<int, SteelArms> guns = new Dictionary<int, SteelArms>();
        private int ActiveItemId = -1;
        private Transform fpc;
        private void Start()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var gun = transform.GetChild(i).GetComponent<SteelArms>();
                guns.Add(gun.GetID(), gun);
            }

            DisableGuns();

            InventoryEventReceiver.ChangeSelectedCellEvent += ChangeGun;
            fpc = FindObjectOfType<FirstPersonController>().transform;
        }

        internal void SetPossibleDestroy(bool v)
        {
            possibleDestroy = v;
        }

        private void ChangeGun(int id)
        {
            if (ActiveItemId != -1)
            {

            }

            ActiveItemId = id;
            DisableGuns();

            if (ActiveItemId == -1)
                return;
        }
        private void DisableGuns()
        {
            foreach (var g in guns)
            {
                g.Value.gameObject.SetActive(false);
            }
            if (guns.ContainsKey(ActiveItemId))
                guns[ActiveItemId].gameObject.SetActive(true);
        }
        private bool animationIsPlayng = false;
        private bool possibleDestroy = false;
        private void Update()
        {
            if (!guns.ContainsKey(ActiveItemId))
                return;

            if (Input.GetMouseButton(0) && !animationIsPlayng)
            {
                animationIsPlayng = true;
                guns[ActiveItemId].GetComponent<Animator>().SetBool("Attack", true);
            }
        }
        private void FixedUpdate()
        {
            if (!guns.ContainsKey(ActiveItemId))
                return;
            if (!possibleDestroy)
                return;
            Ray ray = new Ray(fpc.position, fpc.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, 0.75f, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.TryGetComponent<EnemyCollision>(out var e))
                {
                    e.InjureEnemy(100);
                    CreateImpact(hit);
                    FinishAnimation();
                }
            }
        }
        public void FinishAnimation()
        {
            animationIsPlayng = false;
            guns[ActiveItemId].GetComponent<Animator>().SetBool("Attack", false);
            possibleDestroy = false;
        }
        private void CreateImpact(RaycastHit hit)
        {
            var i = Instantiate(Resources.Load<GameObject>("WeaponEffects\\Prefabs\\BulletImpactFleshSmallEffect"), hit.transform);
            i.transform.position = hit.point;
            i.transform.forward = hit.normal;
        }
    }
}