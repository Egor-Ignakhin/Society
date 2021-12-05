
using UnityEngine;

namespace PathCreation.Examples
{
    public class CinematicPathFollower : MonoBehaviour
    {
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] private EndOfPathInstruction endOfPathInstruction;
        [SerializeField] private float speed = 5;
        private Transform playerCameraTr;
        public void SetSpeed(float value) => speed = value;

        public void SetPathCreator(PathCreator value) => pathCreator = value;

        private void Start() => playerCameraTr = Camera.main.transform;

        private float time;

        private void Update()
        {
            if (pathCreator != null)
            {
                time += speed * Time.deltaTime;
                playerCameraTr.position = pathCreator.path.GetPointAtTime(time, endOfPathInstruction);
                playerCameraTr.rotation = pathCreator.path.GetRotation(time, endOfPathInstruction);
            }
        }
    }
}