using Shears.Input;
using System.Collections;
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
        [SerializeField] private float driftHopForce = 500.0f;
        [SerializeField] private float defaultGrip = 1.0f;
        [SerializeField] private float driftGrip = 1.0f;
        [SerializeField] private float minDriftSlip = 0.15f;
        [SerializeField] private float maxDriftSlip = 0.35f;
        [SerializeField] private float minDriftSteeringAngle = 5.0f;
        [SerializeField] private float maxDriftSteeringAngle = 20.0f;
        [SerializeField] private float maxSpeedDriftSteeringAngle = 8.0f;

        [Header("Chassis Settings")]
        [SerializeField] private float normalChassisRotation = 10.0f;
        [SerializeField] private float driftChassisRotation = 30.0f;
        [SerializeField] private float chassisRotationSpeed = 5.0f;

        private float moveInputDirection;
        private float angularDamping;
        private bool isDrifting = false;
        private Coroutine driftCoroutine;

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

            angularDamping = car.Rigidbody.angularDamping;
            car.Rigidbody.angularDamping = 5.0f;
            car.Rigidbody.AddForce(driftHopForce * Vector3.up, ForceMode.Impulse);
            wheelController.LockWheelRotation();
            wheelController.LockWheelRotationForDrifting(minDriftSteeringAngle, maxDriftSteeringAngle, maxSpeedDriftSteeringAngle);

            if (driftCoroutine != null)
            {
                StopCoroutine(driftCoroutine);
                driftCoroutine = null;
            }

            driftCoroutine = StartCoroutine(IEBeginDrift());
        }

        private void OnDriftInputEnd(ManagedInputInfo info)
        {
            if (driftCoroutine != null)
                StopCoroutine(driftCoroutine);

            car.Rigidbody.angularDamping = angularDamping;
            wheelController.UnlockWheelRotation();
            wheelController.UnlockWheelDrifting();
            moveInputDirection = 0.0f;
            isDrifting = false;

            if (!isDrifting)
                return;

            foreach (var wheel in gripDriftWheels)
                wheel.Grip = defaultGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = defaultGrip;
        }

        private IEnumerator IEBeginDrift()
        {
            while (car.IsOnGround())
                yield return null;

            while (!car.IsOnGround())
                yield return null;

            wheelController.UnlockWheelRotation();

            foreach (var wheel in gripDriftWheels)
                wheel.Grip = driftGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = minDriftSlip;

            isDrifting = true;
        }

        private void UpdateDrift()
        {
            float currentInput = car.Input.MoveInput.ReadValue<Vector2>().x;
            float t = 1.0f - (0.5f * Mathf.Abs(moveInputDirection - currentInput));
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
