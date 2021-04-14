using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Debugger
{
    public class DebugManager : MonoBehaviour
    {
        [SerializeField] private Transform background;
        private bool isHidden = true;
        private bool isMoving = false;
        [SerializeField] private Vector3 hiddenPos;
        [SerializeField] private Vector3 ShowingPos;
        private IDebug activeDebugger;
        [SerializeField] private List<GameObject> DebuggersObjects = new List<GameObject>();
        private readonly List<IDebug> Debuggers = new List<IDebug>();
        private void Awake()
        {
            foreach (var d in DebuggersObjects)
            {
                Debuggers.Add(d.GetComponent<IDebug>());
            }
            
            DisableDebuggers();
            activeDebugger = Debuggers[0];
        }
        private void Update()
        {
            if (isMoving)
                Move();
            if (Input.GetKeyDown(KeyCode.F1))
            {
                isHidden = !isHidden;
                isMoving = true;
            }
        }
        private void DisableDebuggers()
        {
            foreach (var d in Debuggers)
            {
                d.Active = false;
                d.gameObject.SetActive(false);
            }
        }
        public void SetActiveDebugger(GameObject ad)
        {
            DisableDebuggers();
            activeDebugger = ad.GetComponent<IDebug>();            
            activeDebugger.Activate();
        }
        private void Move()
        {
            Vector3 direction;
            if (isHidden)
            {
                direction = hiddenPos;
            }
            else
                direction = ShowingPos;
            if (background.localPosition != direction)
            {
                background.localPosition = Vector3.MoveTowards(background.localPosition, direction, 100);
            }
            else
            {
                background.gameObject.SetActive(!isHidden);
                if (!isHidden)
                {
                    InputManager.LockInput();
                    InputManager.DisableInput();
                }
                else
                {
                    InputManager.Unlock();
                    InputManager.EnableInput();
                }
                isMoving = false;
                activeDebugger.Activate();
            }
        }
    }
}