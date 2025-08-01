using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody carRigidBody;
        [SerializeField] private CarInput carInput;

        [Header("Collision Settings")]
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private float raycastDistance = 0.5f;

        [Header("Suspension Settings")]
        [SerializeField] private float springRestDistance = 0.5f;
        [SerializeField] private float springStrength = 5000f;
        [SerializeField] private float springDamping = 100f;

        [Header("Steering Settings")]
        [SerializeField] private float grip = 0.5f;

        [Header("Acceleration Settings")]
        [SerializeField] private float maxSpeed = 300f;

        private IManagedInput moveInput;

        private void Awake()
        {
            moveInput = carInput.MoveInput;
        }

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, raycastDistance, collisionLayer))
            {
                ApplySuspension(hit);
                ApplySteering();
                ApplyAcceleration();
            }
        }

        private void ApplySuspension(RaycastHit hit)
        {
            Vector3 springDirection = transform.up;
            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(transform.position);

            float offset = springRestDistance - hit.distance;
            float velocity = Vector3.Dot(springDirection, tireWorldVel);

            float force = (offset * springStrength) - (velocity * springDamping);

            carRigidBody.AddForceAtPosition(springDirection * force, transform.position);
        }

        private void ApplySteering()
        {
            Vector3 steeringDirection = transform.right;
            Vector3 tireWorldVelocity = carRigidBody.GetPointVelocity(transform.position);

            float steeringVelocity = Vector3.Dot(steeringDirection, tireWorldVelocity);
            float desiredVelocityChange = -steeringVelocity * grip;
            float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;
            float tireMass = 0.25f * carRigidBody.mass;

            carRigidBody.AddForceAtPosition(steeringDirection * tireMass * desiredAcceleration, transform.position);
        }

        private void ApplyAcceleration()
        {
            Vector3 accelDirection = transform.forward;
            float accelInput = moveInput.ReadValue<Vector2>().y;

            if (Mathf.Abs(accelInput) < 0.1f)
                return;

            //float carSpeed = Vector3.Dot(carRigidBody.transform.forward, carRigidBody.linearVelocity);
            float availableTorque = accelInput * maxSpeed;

            carRigidBody.AddForceAtPosition(accelDirection * availableTorque, transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * raycastDistance);
        }
    }
}
