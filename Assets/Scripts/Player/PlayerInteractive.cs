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
        float interctionDistance = 2;
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
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(rayStartPos);
            string desc = string.Empty;
            if (Physics.Raycast(ray, out hit, interctionDistance, interactionLayer))
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
    }
}