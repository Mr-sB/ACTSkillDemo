using UnityEngine;

namespace ACTSkillDemo
{
    public class CameraView : MonoBehaviour
    {
        public Transform Target;
        public Vector3 TaregtOffset;
        public Vector3 FollowOffset = new Vector3(0, 0, -5);
        public float RotateSpeed = 180;
        private float angleY;
        private Quaternion oriRotation;

        private void Awake()
        {
            oriRotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            if (!Target) return;
            Vector3 offset = HandleRotation(Time.fixedDeltaTime);
            Vector3 followPos = Target.position + TaregtOffset;
            transform.position = followPos + offset;
        }

        private Vector3 HandleRotation(float deltaTime)
        {
            if (Input.GetKey(KeyCode.Q))
                angleY += RotateSpeed * deltaTime;

            if (Input.GetKey(KeyCode.E))
                angleY -= RotateSpeed * deltaTime;

            var rot = Quaternion.Euler(0, angleY, 0);
            transform.rotation = rot * oriRotation;
            return rot * FollowOffset;
        }
    }
}
