using UnityEngine;

namespace CreationWasteland
{
    public class CameraRotate : MonoBehaviour
    {
        public float speed = 1;
        public float radius = 0.5f;
        public float cameraHeight = 0.1f;
        public Transform target;
        private float cycleProgress;

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            cycleProgress += Time.deltaTime * speed;

            this.transform.position = target.transform.position + new Vector3(Mathf.Cos(cycleProgress), cameraHeight, Mathf.Sin(cycleProgress)) * radius;
            this.transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.up);
        }
    }

}