using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class DriftController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CarInput carInput;
        [SerializeField] private Wheel[] gripDriftWheels;
        [SerializeField] private Wheel[] slipDriftWheels;

        [Header("Settings")]
        [SerializeField] private float defaultGrip = 1.0f;
        [SerializeField] private float driftGrip = 1.0f;
        [SerializeField] private float driftSlip = 0.2f;

        private void OnEnable()
        {
            carInput.DriftInput.Started += OnDriftInputBegin;
            carInput.DriftInput.Canceled += OnDriftInputEnd;
        }

        private void OnDisable()
        {
            carInput.DriftInput.Started -= OnDriftInputBegin;
            carInput.DriftInput.Canceled -= OnDriftInputEnd;
        }

        private void Awake()
        {
            foreach (var wheel in gripDriftWheels)
                wheel.Grip = defaultGrip;

            foreach (var wheel in slipDriftWheels)
                wheel.Grip = defaultGrip;
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
    }
}
