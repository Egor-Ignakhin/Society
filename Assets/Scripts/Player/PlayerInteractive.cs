using System;
using UnityEngine;

namespace PlayerClasses
{
    public sealed class PlayerInteractive : MonoBehaviour
    {
        Camera mainCamera;
        private float interctionDistance = 2;
        private const float sphereCasterRadius = 0.01f;
        public static KeyCode InputInteractive { get; set; } = KeyCode.F;
        private bool inputedButton = false;
        private Inventory.DescriptionDrawer descriptionDrawer;
        private Vector3 lasHitPoint;
        private void Start()
        {
            mainCamera = GetComponent<FirstPersonController>().GetCamera();
            descriptionDrawer = Inventory.DescriptionDrawer.Instance;
            descriptionDrawer.SetHint(desc, mainDesc, 0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(InputInteractive))
                inputedButton = true;
            else if (Input.GetKeyUp(InputInteractive))
                inputedButton = false;
        }
        private void FixedUpdate() => RayThrow();

        string desc = string.Empty;
        string mainDesc = string.Empty;
        private void RayThrow()
        {
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            int count = 1;
            desc = string.Empty;
            mainDesc = string.Empty;
            if (Physics.SphereCast(ray.origin, sphereCasterRadius, ray.direction, out RaycastHit hit, interctionDistance, ~0))
            {
                var components = hit.transform.GetComponents<InteractiveObject>();
                if (components.Length > 0)
                {
                    lasHitPoint = hit.point;
                    int i = 0;
                    foreach (var c in components)
                    {
                        string getDesc = c.Description;
                        string getMainDesc = c.MainDescription;

                        int getCount = c is InventoryItem ? (c as InventoryItem).GetCount() : 1;
                        if (!string.IsNullOrEmpty(getDesc))
                        {
                            desc = getDesc;
                            mainDesc = getMainDesc;
                            count = getCount;
                        }

                        if (inputedButton)
                        {
                            c.Interact();
                            if (++i == components.Length)
                                inputedButton = false;
                        }
                    }
                }
            }
            descriptionDrawer.SetHint(desc, mainDesc, count);
        }
        internal Vector3 GetHitPoint() => lasHitPoint;
        /*private void OnDrawGizmos()
        {
            try
            {
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
                bool isHit = Physics.SphereCast(ray.origin, sphereCasterRadius, ray.direction, out RaycastHit hit, interctionDistance, interactionLayer);
                if (isHit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(ray.origin + ray.direction * hit.distance, 0.1f);
                }
            }
            catch
            {
            }
        }*/
    }
}