using UnityEngine;

namespace LostResort.Cars
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private CarMovementData movementData;
        [SerializeField] private float minSpeed = 0.01f;

        private void FixedUpdate()
        {
            Vector3 horizontalVelocity = rigidBody.linearVelocity;
            horizontalVelocity.y = 0;

            if (horizontalVelocity.sqrMagnitude < minSpeed * minSpeed)
            {
                Vector3 verticalVelocity = rigidBody.linearVelocity;
                verticalVelocity.x = 0;
                verticalVelocity.z = 0;
                rigidBody.linearVelocity = verticalVelocity;
            }
        }

        public float GetMaxSpeedPercentage()
        {
            return rigidBody.linearVelocity.sqrMagnitude / (movementData.MaxSpeed * movementData.MaxSpeed);
        }
    }
}
