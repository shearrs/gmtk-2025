using UnityEngine;

namespace LostResort.Cars
{
    public class WheelController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CarInput carInput;
        [SerializeField] private Wheel[] wheels;

        [Header("Settings")]
        [SerializeField] private float handling = 30f;
        [SerializeField] private float maxSteeringAngle = 30f;
        [SerializeField] private float maxSpeedSteeringAngle = 10f;

        private void Update()
        {
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            var rotationInput = carInput.MoveInput.ReadValue<Vector2>().x;

            foreach (var wheel in wheels)
            {
                Vector3 euler = wheel.transform.localEulerAngles;
                float targetRot = rotationInput * maxSteeringAngle;
                
                euler.y = Mathf.MoveTowardsAngle(euler.y, targetRot, handling * Time.deltaTime);

                wheel.transform.localEulerAngles = euler;
            }
        }
    }
}
