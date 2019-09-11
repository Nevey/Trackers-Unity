using Trackers.Utils;
using UnityEngine;

namespace Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float maxMovementSpeed;

        private Rigidbody rigidbody;
        
        private Vector3 moveInput;
        private Vector3 moveDirection;

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");

            Vector3 rightVector = RotationUtil.GetVector(0, transform.eulerAngles.y, 0, Vector3.right, transform.position, Vector3.one);
            Vector3 right = rightVector - transform.position;

            Vector3 forwardVector = RotationUtil.GetVector(0, transform.eulerAngles.y, 0, Vector3.forward,
                transform.position, Vector3.one);
            Vector3 forward = forwardVector - transform.position;
            
            moveDirection = right * moveInput.x + forward * moveInput.z;

            rigidbody.velocity += moveDirection;
            rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxMovementSpeed);
        }
    }
}