using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class Wheel : MonoBehaviour
    {
        [Header("Car References")]
        [SerializeField] private Rigidbody carRigidBody;
        [SerializeField] private CarInput carInput;
        [SerializeField] private CarMovementData movementData;

        [Header("Collision Settings")]
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private float raycastDistance = 0.5f;

        [Header("Suspension Settings")]
        [SerializeField] private float springRestDistance = 0.5f;
        [SerializeField] private float springStrength = 5000f;
        [SerializeField] private float springDamping = 100f;

        [Header("Steering Settings")]
        [SerializeField] private float grip = 0.5f;
        [SerializeField] private float drag = 500f;

        private IManagedInput moveInput;
        private float accelerationInput;

        public float Grip { get => grip; set => grip = value; }

        private void Awake()
        {
            moveInput = carInput.MoveInput;
        }

        private void Update()
        {
            accelerationInput = moveInput.ReadValue<Vector2>().y;
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

            carRigidBody.AddForceAtPosition(desiredAcceleration * tireMass * steeringDirection, transform.position);
        }

        private void ApplyAcceleration()
        {
            Vector3 accelDirection = transform.forward;

            if (Mathf.Abs(accelerationInput) < 0.1f)
            {
                ApplyDrag();
                return;
            }

            float carSpeed = Vector3.Dot(carRigidBody.transform.forward, carRigidBody.linearVelocity);
            float availableTorque = accelerationInput * movementData.MaxSpeed;

            if (Mathf.Sign(accelerationInput) != Mathf.Sign(carSpeed))
                ApplyDrag();

            carRigidBody.AddForceAtPosition(accelDirection * availableTorque, transform.position);
        }

        private void ApplyDrag()
        {
            var carVel = carRigidBody.linearVelocity;
            var dragForce = (drag * -carVel);

            carRigidBody.AddForceAtPosition(dragForce, transform.position);
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
