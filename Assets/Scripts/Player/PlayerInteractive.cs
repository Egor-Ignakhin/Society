using UnityEngine;

namespace PlayerClasses
{
    public sealed class PlayerInteractive : MonoBehaviour
    {
        private void Start()
        {
            mainCamera = Camera.main;
            playerStatements = GetComponent<PlayerStatements>();
        }

        Camera mainCamera;
        private float interctionDistance = 2;
        private const float sphereCasterRadius = 0.1f;
        [SerializeField] LayerMask interactionLayer;
        private KeyCode inputInteractive = KeyCode.F;
        private PlayerStatements playerStatements;
        private Vector3 rayStartPos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        private bool inputedButton = false;

        private void Update()
        {
            if (Input.GetKeyDown(inputInteractive))
            {
                inputedButton = true;
            }
            else
            if (Input.GetKeyUp(inputInteractive))
            {
                inputedButton = false;
            }
        }
        private void FixedUpdate()
        {
            RayThrow();
        }
        private void RayThrow()
        {
            Ray ray = mainCamera.ScreenPointToRay(rayStartPos);
            string desc = string.Empty;
            if (Physics.SphereCast(ray.origin, sphereCasterRadius, ray.direction, out RaycastHit hit, interctionDistance, interactionLayer))
            {
                var components = hit.transform.GetComponents<InteractiveObject>();
                int i = 0;
                foreach (var c in components)
                {
                    string getDesc = c.GetDescription();
                    if (!string.IsNullOrEmpty(getDesc))
                        desc = getDesc;

                    if (inputedButton)
                    {
                        c.Interact(playerStatements);
                        if (++i == components.Length)
                            inputedButton = false;
                    }
                }
            }
            DescriptionDrawer.Instance.SetHint(desc);
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