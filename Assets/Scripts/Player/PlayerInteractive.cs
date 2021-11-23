using Society.Inventory;
using Society.Inventory.Other;
using Society.Patterns;
using Society.Player.Controllers;
using Society.Settings;

using System.Linq;

using UnityEngine;

namespace Society.Player
{
    public sealed class PlayerInteractive : MonoBehaviour
    {
        private Camera mainCamera;
        private const float interctionDistance = 2;
        private const float sphereCasterRadius = 0.01f;
        private const float sphSCastRadiusMultiply = 10;
        private bool inputedButton = false;
        private DescriptionDrawer descriptionDrawer;
        private Vector3 lasHitPoint;
        private InteractiveObject[] directedObjects = new InteractiveObject[0];
        private SupportItemCircular supportItemCircular;
        private Ray tempRay;
        private Vector3 tempInteractionPoint;
        [SerializeField] private LayerMask interactionLayers;
        private void Start()
        {
            mainCamera = GetComponent<FirstPersonController>().GetCamera();
            descriptionDrawer = FindObjectOfType<DescriptionDrawer>();
            descriptionDrawer.SetHint(desc, mainDesc, 0);
            supportItemCircular = FindObjectOfType<SupportItemCircular>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(GameSettings.GetInteractionKeyCode()))
                inputedButton = true;
            else if (Input.GetKeyUp(GameSettings.GetInteractionKeyCode()))
                inputedButton = false;
        }

        internal bool ObjectIsDirected(InteractiveObject io) =>
             (directedObjects != null) && directedObjects.Contains(io);


        private void FixedUpdate()
        {
            tempRay = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RayThrow();
            RayThrowForActivateSuppurtCurcular();
        }

        private string desc = string.Empty;
        private string mainDesc = string.Empty;
        private void RayThrow()
        {
            int count = 1;
            desc = string.Empty;
            mainDesc = string.Empty;
            directedObjects = null;
            if (Physics.SphereCast(tempRay.origin, sphereCasterRadius,
                tempRay.direction, out RaycastHit hit, interctionDistance, interactionLayers, QueryTriggerInteraction.Ignore))
            {
                directedObjects = hit.transform.GetComponents<InteractiveObject>();
                if (directedObjects.Length > 0)
                {
                    lasHitPoint = hit.point;
                    int i = 0;
                    foreach (var c in directedObjects)
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
                            if (++i == directedObjects.Length)
                                inputedButton = false;
                        }
                    }
                }
                tempInteractionPoint = hit.point;
            }
            descriptionDrawer.SetHint(desc, mainDesc, count);
        }

        /// <summary>
        /// Задачётся и регулируется прозрачность 
        /// </summary>
        private void RayThrowForActivateSuppurtCurcular()
        {
            float tempOpacity = 0;
            //float max            
            if (Physics.SphereCast(tempRay.origin, sphereCasterRadius * sphSCastRadiusMultiply,
                tempRay.direction, out RaycastHit hit, interctionDistance, interactionLayers, QueryTriggerInteraction.Ignore))// Бросок сфера удвоенного радиуса
            {
                if (hit.transform.GetComponent<InteractiveObject>())
                {
                    tempOpacity = 1 / Vector3.Distance(hit.point, tempInteractionPoint) / 100;
                }
            }

            if (directedObjects != null && (directedObjects.Length > 0))
                supportItemCircular.SetColorByTypeOfInteractiveObject(directedObjects);
            supportItemCircular.SetSpriteOpacity(tempOpacity);
        }
        internal Vector3 GetHitPoint() => lasHitPoint;
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            #region drawSphereForDefaultInteraction
            try
            {
                bool isHit = Physics.SphereCast(tempRay.origin, sphereCasterRadius, tempRay.direction, out RaycastHit hit, interctionDistance, interactionLayers);
                if (isHit)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(tempRay.origin + tempRay.direction * hit.distance, sphereCasterRadius);
                }
            }
            catch
            {
            }
            #endregion
            #region DrawSphereForSPCircularInteraction
            try
            {
                bool isHit = Physics.SphereCast(tempRay.origin, sphereCasterRadius * sphSCastRadiusMultiply, tempRay.direction, out RaycastHit hit, interctionDistance, interactionLayers);
                if (isHit)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(tempRay.origin + tempRay.direction * hit.distance, sphereCasterRadius * sphSCastRadiusMultiply);
                }
            }
            catch
            {
            }
            #endregion
        }
#endif
    }
}