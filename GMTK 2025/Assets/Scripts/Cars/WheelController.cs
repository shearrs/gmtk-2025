using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class WheelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Car car;
        [SerializeField] private CarInput carInput;
        [SerializeField] private Rigidbody carRigidBody;
        [SerializeField] private Wheel[] wheels;

        [Header("Settings")]
        [SerializeField] private float handling = 30f;
        [SerializeField] private float maxSteeringAngle = 30f;
        [SerializeField] private float maxSpeedSteeringAngle = 10f;

        private Vector3 rotationInput;
        private bool isLocked = false;
        private float lockedRotationInput;
        private Vector2 lockedRotationRange;

        private void Update()
        {
            rotationInput = carInput.MoveInput.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            UpdateRotation();
        }

        public void LockWheelRotationForDrifting(float minSteeringAngle, float maxSteeringAngle)
        {
            if (isLocked)
                return;

            float rotDirection = rotationInput.x;
            lockedRotationRange = new(rotDirection * minSteeringAngle, rotDirection * maxSteeringAngle);
            lockedRotationInput = Mathf.Sign(rotDirection);

            isLocked = true;
        }

        public void UnlockWheelRotation()
        {
            isLocked = false;
        }

        private void UpdateRotation()
        {
            if (isLocked)
            {
                foreach (var wheel in wheels)
                {
                    Vector3 euler = wheel.transform.localEulerAngles;

                    float inputT = 1.0f - (0.5f * Mathf.Abs(lockedRotationInput - rotationInput.x));
                    float steerAngle = Mathf.Lerp(lockedRotationRange.x, lockedRotationRange.y, inputT);

                    euler.y = Mathf.MoveTowardsAngle(euler.y, steerAngle, handling * Time.fixedDeltaTime);

                    wheel.transform.localEulerAngles = euler;
                }

                return;
            }

            foreach (var wheel in wheels)
            {
                Vector3 euler = wheel.transform.localEulerAngles;

                float velocityT = car.GetMaxSpeedPercentage();
                float maxSteerAngle = Mathf.Lerp(maxSteeringAngle, maxSpeedSteeringAngle, velocityT);

                float currentHandling = handling;
                float targetRot = maxSteerAngle * rotationInput.x;

                euler.y = Mathf.MoveTowardsAngle(euler.y, targetRot, currentHandling * Time.fixedDeltaTime);

                wheel.transform.localEulerAngles = euler;
            }
        }
    }
}
