using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class DriftController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Car car;
        [SerializeField] private WheelController wheelController;
        [SerializeField] private Wheel[] gripDriftWheels;
        [SerializeField] private Wheel[] slipDriftWheels;
        [SerializeField] private Transform chassis;
        
        [Header("Drift Settings")]
        [SerializeField] private float defaultGrip = 1.0f;
        [SerializeField] private float driftGrip = 1.0f;
        [SerializeField] private float minDriftSlip = 0.15f;
        [SerializeField] private float maxDriftSlip = 0.35f;
        [SerializeField] private float minDriftSteeringAngle = 10.0f;
        [SerializeField] private float maxDriftSteeringAngle = 20.0f;

        [Header("Chassis Settings")]
        [SerializeField] private float normalChassisRotation = 10.0f;
        [SerializeField] private float driftChassisRotation = 30.0f;
        [SerializeField] private float chassisRotationSpeed = 5.0f;

        private float moveInputDirection;
        private bool isDrifting = false;

        private void OnEnable()
        {
            car.Input.DriftInput.Started += OnDriftInputBegin;
            car.Input.DriftInput.Canceled += OnDriftInputEnd;
        }

        private void OnDisable()
        {
            car.Input.DriftInput.Started -= OnDriftInputBegin;
            car.Input.DriftInput.Canceled -= OnDriftInputEnd;
        }

        private void Awake()
        {
            foreach (var wheel in gripDriftWheels)
                wheel.Grip = defaultGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = defaultGrip;
        }

        private void Update()
        {
            UpdateChassisRotation();

            if (isDrifting)
                UpdateDrift();
        }

        private void OnDriftInputBegin(ManagedInputInfo info)
        {
            moveInputDirection = car.Input.MoveInput.ReadValue<Vector2>().x;

            if (Mathf.Abs(moveInputDirection) < 0.01f)
                return;

            moveInputDirection = Mathf.Sign(moveInputDirection);
            isDrifting = true;
            wheelController.LockWheelRotationForDrifting(minDriftSteeringAngle, maxDriftSteeringAngle);

            foreach (var wheel in gripDriftWheels)
                wheel.Grip = driftGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = minDriftSlip;
        }

        private void OnDriftInputEnd(ManagedInputInfo info)
        {
            if (!isDrifting)
                return;

            isDrifting = false;
            moveInputDirection = 0.0f;
            wheelController.UnlockWheelRotation();

            foreach (var wheel in gripDriftWheels)
                wheel.Grip = defaultGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = defaultGrip;
        }

        private void UpdateDrift()
        {
            float currentInput = car.Input.MoveInput.ReadValue<Vector2>().x;
            float t = (2.0f * (moveInputDirection - currentInput)) - 1.0f;
            float driftSlip = Mathf.Lerp(minDriftSlip, maxDriftSlip, t);

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = driftSlip;
        }

        private void UpdateChassisRotation()
        {
            float maxRotation = isDrifting ? driftChassisRotation : normalChassisRotation;
            maxRotation *= car.GetMaxSpeedPercentage() * moveInputDirection;

            Vector3 rotation = chassis.localEulerAngles;
            float angle = rotation.z;

            rotation.z = Mathf.LerpAngle(angle, maxRotation, chassisRotationSpeed * Time.deltaTime);

            chassis.localEulerAngles = rotation;
        }
    }
}
