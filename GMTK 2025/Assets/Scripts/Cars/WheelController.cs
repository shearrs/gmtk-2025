using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class WheelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CarInput carInput;
        [SerializeField] private Rigidbody carRigidBody;
        [SerializeField] private CarMovementData movementData;
        [SerializeField] private Wheel[] wheels;

        [Header("Settings")]
        [SerializeField] private float handling = 30f;
        [SerializeField] private float maxSteeringAngle = 30f;
        [SerializeField] private float maxSpeedSteeringAngle = 10f;

        private Vector3 rotationInput;

        private void Update()
        {
            rotationInput = carInput.MoveInput.ReadValue<Vector2>();
        }

        private void FixedUpdate()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            foreach (var wheel in wheels)
            {
                Vector3 euler = wheel.transform.localEulerAngles;

                float velocityT = carRigidBody.linearVelocity.sqrMagnitude / (movementData.MaxSpeed * movementData.MaxSpeed);
                Debug.Log("t: " + velocityT);
                float maxSteerAngle = Mathf.Lerp(maxSteeringAngle, maxSpeedSteeringAngle, velocityT);
                float targetRot = rotationInput.x * maxSteerAngle;
                float currentHandling = handling;



                euler.y = Mathf.MoveTowardsAngle(euler.y, targetRot, currentHandling * Time.fixedDeltaTime);

                wheel.transform.localEulerAngles = euler;
            }
        }
    }
}
