using UnityEngine;

namespace PlayerClasses
{
    public sealed class PlayerInteractive : MonoBehaviour
    {
        Camera mainCamera;
        private float interctionDistance = 2;
        private const float sphereCasterRadius = 0.1f;
        [SerializeField] LayerMask interactionLayer;
        public static KeyCode InputInteractive { get; set; } = KeyCode.F;
        private PlayerStatements playerStatements;
        private bool inputedButton = false;
        private DescriptionDrawer descriptionDrawer;
        private void Start()
        {
            mainCamera = Camera.main;
            playerStatements = GetComponent<PlayerStatements>();
            descriptionDrawer = DescriptionDrawer.Instance;
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
            if (Physics.SphereCast(ray.origin, sphereCasterRadius, ray.direction, out RaycastHit hit, interctionDistance, interactionLayer))
            {
                var components = hit.transform.GetComponents<InteractiveObject>();
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
                        c.Interact(playerStatements);
                        if (++i == components.Length)
                            inputedButton = false;
                    }
                }
            }
            descriptionDrawer.SetHint(desc, mainDesc, count);
        }
        /*   void OnDrawGizmos()
           {
               try
               {
                   Ray ray = mainCamera.ScreenPointToRay(rayStartPos);
                   bool isHit = Physics.SphereCast(ray.origin, 0.1f, ray.direction, out RaycastHit hit, interctionDistance, interactionLayer);
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