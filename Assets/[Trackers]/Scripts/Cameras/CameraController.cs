using Trackers.Utils;
using UnityEngine;

namespace Trackers.Cameras
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform mainTarget;

        [SerializeField] private float minRotationX = -90f;
        [SerializeField] private float maxRotationX = 90f;

        [Header("Sensitivity Settings")] 
        [SerializeField] private Vector2 lookSensitivity = new Vector2(150f, 150f);

        private Vector2 lookRotation;
        private Vector2 lookVelocity;
        
        private Vector3 targetPosition;
        private Vector3 positionVel;
        
        private Quaternion targetRotation;
        private float xVel;
        private float yVel;
        private float zVel;

        private Vector2 previousInput;

        private void Awake()
        {
            if (mainTarget == null)
            {
                throw Log.Exception("No Main Target is set!");
            }

            lookRotation = transform.parent.eulerAngles;
            
            previousInput.x = Input.mousePosition.x;
            previousInput.y = Input.mousePosition.y;
        }

        private void Update()
        {
            if (mainTarget == null)
            {
                return;
            }

            Vector2 input = Input.mousePosition;
            input.x -= previousInput.x;
            input.y -= previousInput.y;

            input.x = ClampUtil.Angle(input.x, minRotationX, maxRotationX);

            lookRotation.x += input.y * lookSensitivity.y * Time.deltaTime;
            lookRotation.y += input.x * lookSensitivity.x * Time.deltaTime;

            lookRotation.x = ClampUtil.Angle(lookRotation.x, minRotationX, maxRotationX);

            Quaternion xQuaternion = Quaternion.AngleAxis(lookRotation.y, Vector3.up);
            Quaternion yQuaternion = Quaternion.AngleAxis(lookRotation.x, Vector3.left);

            transform.parent.rotation = xQuaternion * yQuaternion;

            previousInput.x = Input.mousePosition.x;
            previousInput.y = Input.mousePosition.y;
        }
    }
}