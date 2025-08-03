using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class Wheel : MonoBehaviour
    {
        [Header("Car References")]
        [SerializeField] private Car car;
        [SerializeField] private Transform model;

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


        [Header("Model Settings")]
        [SerializeField] private float modelPadding = 0.45f;
        [SerializeField] private float modelFallSpeed = 10.0f;

        private IManagedInput moveInput;
        private float accelerationInput;

        public float Grip { get => grip; set => grip = value; }

        private void Awake()
        {
            moveInput = car.Input.MoveInput;
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
                SnapModelToFloorPos(hit.point);
            }
            else
                ApplyGravityToModel();
        }

        public bool IsOnGround()
        {
            return Physics.Raycast(transform.position, -transform.up, raycastDistance, collisionLayer);
        }

        public RaycastHit GetRaycastHit()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, raycastDistance, collisionLayer))
            {
                return hit;
            }

            return new RaycastHit();
        }

        private void ApplySuspension(RaycastHit hit)
        {
            Vector3 springDirection = transform.up;
            Vector3 tireWorldVel = car.Rigidbody.GetPointVelocity(transform.position);

            float offset = springRestDistance - hit.distance;
            float velocity = Vector3.Dot(springDirection, tireWorldVel);

            float force = (offset * springStrength) - (velocity * springDamping);

            car.Rigidbody.AddForceAtPosition(springDirection * force, transform.position);
        }

        private void ApplySteering()
        {
            Vector3 steeringDirection = transform.right;
            Vector3 tireWorldVelocity = car.Rigidbody.GetPointVelocity(transform.position);

            float steeringVelocity = Vector3.Dot(steeringDirection, tireWorldVelocity);
            float desiredVelocityChange = -steeringVelocity * grip;
            float desiredAcceleration = desiredVelocityChange / Time.fixedDeltaTime;
            float tireMass = 0.25f * car.Rigidbody.mass;

            car.Rigidbody.AddForceAtPosition(desiredAcceleration * tireMass * steeringDirection, transform.position);
        }

        private void ApplyAcceleration()
        {
            Vector3 accelDirection = transform.forward;

            if (Mathf.Abs(accelerationInput) < 0.1f)
            {
                ApplyDrag();
                return;
            }

            float carSpeed = Vector3.Dot(car.Rigidbody.transform.forward, car.Rigidbody.linearVelocity);
            float availableTorque = accelerationInput * car.MovementData.Acceleration;

            if (Mathf.Sign(accelerationInput) != Mathf.Sign(carSpeed))
                ApplyDrag();

            car.Rigidbody.AddForceAtPosition(accelDirection * availableTorque, transform.position);
        }

        private void ApplyDrag()
        {
            var carVel = car.Rigidbody.linearVelocity;
            var dragForce = (drag * -carVel);

            car.Rigidbody.AddForceAtPosition(dragForce, transform.position);
        }

        private void SnapModelToFloorPos(Vector3 position)
        {
            position.y += modelPadding;
            model.position = position;
        }

        private void ApplyGravityToModel()
        {
            Vector3 targetPos = transform.position;

            if (car.Rigidbody.linearVelocity.y > 0)
                targetPos.y -= modelPadding;

            model.position = Vector3.Lerp(model.position, targetPos, modelFallSpeed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * raycastDistance);

            Gizmos.color = Color.darkGoldenRod;
            Gizmos.DrawRay(transform.position, -transform.up * modelPadding);
        }
    }
}
