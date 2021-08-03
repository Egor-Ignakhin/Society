using System;
using UnityEngine;
namespace Features
{
    /// <summary>
    /// Класс душа. Душом можно управлять: двигать по 2 осям и поливать пол.
    /// </summary>
    public sealed class ShowerGarden : MonoBehaviour
    {
        private bool isDragged;
        private ShowerExample activeShower;
        private Transform activeShowerTR;
        private PlayerClasses.PlayerInteractive playerInteractive;
       

        [SerializeField] private Transform pivot;
        [SerializeField] private float speed = 5;

        [SerializeField] private Transform xRope;// веревка по оси x
        [SerializeField] private Transform zRope;// веревка по оси z

        [SerializeField] private Transform minXPoint;
        [SerializeField] private Transform maxXPoint;
        [SerializeField] private Transform minZPoint;
        [SerializeField] private Transform maxZPoint;
        
        private void Awake()
        {
            playerInteractive = FindObjectOfType<PlayerClasses.PlayerInteractive>();            
        }
        internal void OnInteract(ShowerExample ashower)
        {
            isDragged = true;
            activeShower = ashower;
            activeShowerTR = activeShower.transform;
        }
        private void Update()
        {
            if (isDragged)
            {
                MoveShowerToPlayerPointer();
                MoveRopesToShowerCenter();
                if (!playerInteractive.ObjectIsDirected(activeShower))
                {
                    ClearReferences();
                }
            }
        }

        private void ClearReferences()
        {
            isDragged = false;
            activeShower = null;
            activeShowerTR = null;
        }

        private void MoveShowerToPlayerPointer()
        {
            Vector3 shPos = pivot.position;
            Vector3 target = playerInteractive.GetHitPoint();
            target.y = shPos.y;
            // если расстояние слишком близко
            if (Vector3.Distance(shPos, target) > 0.05f)
            {
                Vector3 nextPos = Vector3.LerpUnclamped(shPos, target, Time.deltaTime * speed);
                nextPos.x = Mathf.Clamp(nextPos.x, minXPoint.position.x, maxXPoint.position.x);
                nextPos.z = Mathf.Clamp(nextPos.z, minZPoint.position.z, maxZPoint.position.z);
                pivot.position = nextPos;

            }
        }
        private void MoveRopesToShowerCenter()
        {
            xRope.position = new Vector3(pivot.position.x, xRope.position.y, xRope.position.z);
            zRope.position = new Vector3(zRope.position.x, zRope.position.y, pivot.position.z);
        }     
    }
}