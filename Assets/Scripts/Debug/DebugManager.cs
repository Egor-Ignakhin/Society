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
        [SerializeField] private DebugConsole console;

        private IDebug activeDebugger;
        [SerializeField] private List<GameObject> DebuggersObjects = new List<GameObject>();
        private List<IDebug> Debuggers = new List<IDebug>();
        private void Awake()
        {
            foreach(var d in DebuggersObjects)
            {
                Debuggers.Add(d.GetComponent<IDebug>());                
            }
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
        public void SetActiveDebugger(GameObject ad)
        {
            foreach (var d in Debuggers)
            {
                d.Active = false;
                d.gameObject.SetActive(false);
            }
            activeDebugger = ad.GetComponent<IDebug>();
            activeDebugger.Active = true;
            ad.SetActive(true);
            Debug.Log(ad.name);
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
                console.Activate();
            }
        }
    }
}