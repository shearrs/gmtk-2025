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
        private bool isDrifting = false;
        private float lockedRotationInput;
        float lockedMinSteeringAngle;
        float lockedMaxSteeringAngle;
        float lockedMaxSpeedSteeringAngle;
        private float lockedHandling;

        private void Update()
        {
            rotationInput = carInput.MoveInput.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            if (!isLocked)
                UpdateRotation();
        }

        public void LockWheelRotation()
        {
            isLocked = true;
        }

        public void UnlockWheelRotation()
        {
            isLocked = false;
        }

        public void LockWheelRotationForDrifting(float minSteeringAngle, float maxSteeringAngle, float maxSpeedSteeringAngle)
        {
            if (isDrifting)
                return;

            float rotDirection = rotationInput.x;
            lockedMinSteeringAngle = rotDirection * minSteeringAngle;
            lockedMaxSteeringAngle = rotDirection * maxSteeringAngle;
            lockedMaxSpeedSteeringAngle = rotDirection * maxSpeedSteeringAngle;
            lockedRotationInput = Mathf.Sign(rotDirection);
            lockedHandling = handling;

            foreach (Wheel wheel in wheels)
            {
                Vector3 euler = wheel.transform.localEulerAngles;
                euler.y = lockedMinSteeringAngle;

                wheel.transform.localEulerAngles = euler;
            }

            isDrifting = true;
        }

        public void UnlockWheelDrifting()
        {
            isDrifting = false;
        }

        private void UpdateRotation()
        {
            if (isDrifting)
            {
                foreach (var wheel in wheels)
                {
                    Vector3 euler = wheel.transform.localEulerAngles;

                    float velocityT = car.GetMaxSpeedPercentage();
                    float maxSteeringAngle = Mathf.Lerp(lockedMaxSteeringAngle, lockedMaxSpeedSteeringAngle, velocityT);

                    float inputT = 1.0f - (0.5f * Mathf.Abs(lockedRotationInput - rotationInput.x));
                    float steerAngle = Mathf.Lerp(lockedMinSteeringAngle, maxSteeringAngle, inputT);

                    euler.y = Mathf.MoveTowardsAngle(euler.y, steerAngle, lockedHandling * Time.fixedDeltaTime);

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
