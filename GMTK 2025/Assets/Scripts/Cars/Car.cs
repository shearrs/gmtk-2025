using UnityEngine;

namespace LostResort.Cars
{
    public class Car : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CarInput input;
        [SerializeField] private CarMovementData movementData;
        [SerializeField] private Wheel[] wheels;

        public Rigidbody Rigidbody => rb;
        public CarInput Input => input;
        public CarMovementData MovementData => movementData;

        private void FixedUpdate()
        {
            ClampHorizontalVelocity();
            ApplyFallSpeed();
        }

        public bool IsOnGround()
        {
            bool isOnGround = true;

            foreach (var wheel in wheels)
            {
                if (!wheel.IsOnGround())
                {
                    isOnGround = false;
                    break;
                }
            }

            return isOnGround;
        }

        private void ApplyFallSpeed()
        {
            if (IsOnGround())
                return;

            rb.AddForce(movementData.FallAcceleration * Vector3.down);
        }

        private void ClampHorizontalVelocity()
        {
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0;

            float horizontalSqrMagnitude = horizontalVelocity.sqrMagnitude;

            if (horizontalSqrMagnitude < movementData.MinSpeed * movementData.MinSpeed)
            {
                Vector3 verticalVelocity = rb.linearVelocity;
                verticalVelocity.x = 0;
                verticalVelocity.z = 0;
                rb.linearVelocity = verticalVelocity;
            }
            else if (horizontalSqrMagnitude > movementData.MaxSpeed * movementData.MaxSpeed)
            {
                Debug.Log("clamped speed");
                Vector3 clampedVelocity = horizontalVelocity.normalized;
                clampedVelocity *= movementData.MaxSpeed;
                clampedVelocity.y = rb.linearVelocity.y;

                rb.linearVelocity = clampedVelocity;
            }
        }

        public float GetMaxSpeedPercentage()
        {
            return rb.linearVelocity.sqrMagnitude / (movementData.MaxSpeed * movementData.MaxSpeed);
        }
    }
}
