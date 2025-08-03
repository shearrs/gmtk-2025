using Shears;
using Shears.Input;
using System;
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
        [SerializeField] private ParticleSystem[] driftParticles;

        [Header("Drift Settings")]
        [SerializeField] private float defaultGrip = 1.0f;
        [SerializeField] private float driftGrip = 1.0f;
        [SerializeField] private float minDriftSlip = 0.15f;
        [SerializeField] private float maxDriftSlip = 0.35f;
        [SerializeField] private float minDriftSteeringAngle = 5.0f;
        [SerializeField] private float maxDriftSteeringAngle = 20.0f;
        [SerializeField] private float maxSpeedDriftSteeringAngle = 8.0f;

        [Header("Drift Speed Settings")]
        [SerializeField] private float driftStartupTime = 0.1f;
        [SerializeField] private float driftSlowdownForce = 100.0f;
        [SerializeField] private float driftSpeedupForce = 200.0f;
        [SerializeField] private float speedupRequiredTime = 1.5f;

        [Header("Chassis Settings")]
        [SerializeField] private float normalChassisRotation = 10.0f;
        [SerializeField] private float driftChassisRotation = 30.0f;
        [SerializeField] private float chassisRotationSpeed = 5.0f;

        private WheelTreadTrail backLeftWheelTreads;
        private WheelTreadTrail backRightWheelTreads;
        private float moveInputDirection;
        private bool isDrifting = false;
        private Coroutine driftCoroutine;
        private readonly Timer speedupTimer = new();

        public event Action BeganDrifting;
        public event Action EndedDrifting;
        public event Action PreformingWhoosh;


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

            if (slipDriftWheels[0].transform.localPosition.x < slipDriftWheels[1].transform.localPosition.x)
            {
                backLeftWheelTreads = slipDriftWheels[0].GetComponentInChildren<WheelTreadTrail>();
                backRightWheelTreads = slipDriftWheels[1].GetComponentInChildren<WheelTreadTrail>();
            }
            else
            {
                backLeftWheelTreads = slipDriftWheels[1].GetComponentInChildren<WheelTreadTrail>();
                backRightWheelTreads = slipDriftWheels[0].GetComponentInChildren<WheelTreadTrail>();
            }
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

            wheelController.LockWheelRotationForDrifting(minDriftSteeringAngle, maxDriftSteeringAngle, maxSpeedDriftSteeringAngle);

            Vector3 forceDirection = -car.Rigidbody.linearVelocity.normalized;
            car.Rigidbody.AddForce(driftSlowdownForce * forceDirection, ForceMode.Impulse);

            speedupTimer.Stop();
            speedupTimer.Start(speedupRequiredTime);

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

            wheelController.UnlockWheelDrifting();
            moveInputDirection = 0.0f;

            if (!isDrifting)
                return;

            if (speedupTimer.IsDone)
            {
                PreformingWhoosh?.Invoke();
                Vector3 forceDirection = car.transform.forward;
                car.Rigidbody.AddForce(driftSpeedupForce * forceDirection, ForceMode.Impulse);
                
            }

            speedupTimer.Stop();

            foreach (var particles in driftParticles)
                particles.Stop();

            foreach (var wheel in gripDriftWheels)
                wheel.Grip = defaultGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = defaultGrip;

            isDrifting = false;
            EndedDrifting?.Invoke();
        }

        private IEnumerator IEBeginDrift()
        {
            yield return CoroutineUtil.WaitForSeconds(driftStartupTime);

            foreach (var wheel in gripDriftWheels)
                wheel.Grip = driftGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = minDriftSlip;

            isDrifting = true;
            BeganDrifting?.Invoke();

            foreach (var particles in driftParticles)
                particles.Play();
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
            

            if (isDrifting && Mathf.DeltaAngle(rotation.z, maxRotation) <= 2.5f)
            {
                if (moveInputDirection > 0)
                {
                    backRightWheelTreads.Disable();
                    backRightWheelTreads.Lock();
                }
                else
                {
                    backLeftWheelTreads.Disable();
                    backLeftWheelTreads.Lock();
                }
            }
            else
            {
                backRightWheelTreads.Unlock();
                backLeftWheelTreads.Unlock();
            }

            chassis.localEulerAngles = rotation;
        }
    }
}
