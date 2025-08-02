using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class DriftController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Car car;
        [SerializeField] private Wheel[] gripDriftWheels;
        [SerializeField] private Wheel[] slipDriftWheels;
        [SerializeField] private Transform chassis;
        
        [Header("Drift Settings")]
        [SerializeField] private float defaultGrip = 1.0f;
        [SerializeField] private float driftGrip = 1.0f;
        [SerializeField] private float driftSlip = 0.2f;

        [Header("Chassis Settings")]
        [SerializeField] private float normalChassisRotation = 10.0f;
        [SerializeField] private float driftChassisRotation = 30.0f;
        [SerializeField] private float chassisRotationSpeed = 5.0f;

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
        }

        private void OnDriftInputBegin(ManagedInputInfo info)
        {
            foreach (var wheel in gripDriftWheels)
                wheel.Grip = driftGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = driftSlip;
        }

        private void OnDriftInputEnd(ManagedInputInfo info)
        {
            foreach (var wheel in gripDriftWheels)
                wheel.Grip = defaultGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = defaultGrip;
        }

        private void UpdateChassisRotation()
        {
            var moveInput = car.Input.MoveInput.ReadValue<Vector2>().x;

            float maxRotation = car.Input.DriftInput.IsPressed() ? driftChassisRotation : normalChassisRotation;
            maxRotation *= car.GetMaxSpeedPercentage() * moveInput;

            Vector3 rotation = chassis.localEulerAngles;
            float angle = rotation.z;

            rotation.z = Mathf.LerpAngle(angle, maxRotation, chassisRotationSpeed * Time.deltaTime);

            chassis.localEulerAngles = rotation;
        }
    }
}
