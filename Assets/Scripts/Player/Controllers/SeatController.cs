using Society.Enviroment.Chair;
using Society.GameScreens;
using Society.Patterns;

using UnityEngine;
namespace Society.Player.Controllers
{
    public sealed class SeatController : MonoBehaviour, IGameScreen
    {
        private bool isSitting;
        private ChairManager lastChairManager;// активная система стульев
        private ChairMesh lastChairMesh;// активный стул    
        private Transform currentParent;
        private FirstPersonController fpc;
        private void Awake()
        {
            fpc = FindObjectOfType<FirstPersonController>();
        }
        private void Start()
        {
            sensitivity = GameSettings.GetSensivity();
        }
        /// <summary>
        /// запись состояния в контроллёр
        /// </summary>
        /// <param name="s"></param>
        /// <param name="manager"></param>
        /// <param name="bMesh"></param>
        internal void SetState(State s, ChairManager manager = null, ChairMesh cMesh = null)
        {
            // запись состояния во время начала сна
            if (manager != null)
                lastChairManager = manager;

            if (cMesh != null)
                lastChairMesh = cMesh;

            // конец записи состояния

            switch (s)
            {
                case State.unlocked:
                    isSitting = false;
                    lastChairManager.RiseUp(lastChairMesh);// заправить кровать                               
                    lastChairMesh = null;
                    lastChairManager = null;
                    ScreensManager.SetScreen(null);
                    currentParent = null;
                    break;

                case State.locked:// в случае укладывания в кровать                
                    ScreensManager.SetScreen(this, false);
                    isSitting = true;
                    currentParent = cMesh.GetSeatPlace();
                    break;
            }
            fpc.SetPossibleJump(false);
        }


        private float sensitivity;
        private readonly float minimumVert = -45.0f;
        private readonly float maximumVert = 45.0f;


        private void Update()
        {
            if (isSitting)
            {
                float rotationX = Input.GetAxis("Mouse Y") * sensitivity;
                float rotationY = Input.GetAxis("Mouse X") * sensitivity;

                currentParent.localEulerAngles += new Vector3(0, rotationY, rotationX);
                if (currentParent.eulerAngles.z < 315 && currentParent.eulerAngles.z > maximumVert * 2)
                    currentParent.eulerAngles = new Vector3(currentParent.eulerAngles.x, currentParent.eulerAngles.y, minimumVert);
                if (currentParent.eulerAngles.z < maximumVert * 2 && currentParent.eulerAngles.z > maximumVert)
                    currentParent.eulerAngles = new Vector3(currentParent.eulerAngles.x, currentParent.eulerAngles.y, maximumVert);
            }
        }

        public void RemoveLastChair()
        {
            if (lastChairManager != null && lastChairMesh != null)
                lastChairManager.DeOccupied(lastChairMesh);
        }

        public bool Hide()
        {
            SetState(State.unlocked);
            return true;
        }

        public KeyCode HideKey() => KeyCode.Space;
    }
}