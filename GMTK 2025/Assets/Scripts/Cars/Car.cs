using UnityEngine;

namespace LostResort.Cars
{
    public class Car : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CarInput input;
        [SerializeField] private CarMovementData movementData;

        public Rigidbody Rigidbody => rb;
        public CarInput Input => input;
        public CarMovementData MovementData => movementData;

        private void FixedUpdate()
        {
            Vector3 horizontalVelocity = rb.linearVelocity;
            horizontalVelocity.y = 0;

            if (horizontalVelocity.sqrMagnitude < movementData.MinSpeed * movementData.MinSpeed)
            {
                Vector3 verticalVelocity = rb.linearVelocity;
                verticalVelocity.x = 0;
                verticalVelocity.z = 0;
                rb.linearVelocity = verticalVelocity;
            }
        }

        public float GetMaxSpeedPercentage()
        {
            return rb.linearVelocity.sqrMagnitude / (movementData.MaxSpeed * movementData.MaxSpeed);
        }
    }
}
