using UnityEngine;

namespace LostResort.Cars
{
    [CreateAssetMenu(fileName = "Car Movement Data", menuName = "Lost Resort/Cars/Car Movement Data")]
    public class CarMovementData : ScriptableObject
    {
        [Header("Acceleration Settings")]
        [SerializeField] private float acceleration = 500f;
        [SerializeField] private float maxSpeed = 100f;

        public float Acceleration => acceleration;
        public float MaxSpeed => maxSpeed;
    }
}
