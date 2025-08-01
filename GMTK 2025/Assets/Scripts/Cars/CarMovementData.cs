using UnityEngine;

namespace LostResort.Cars
{
    [CreateAssetMenu(fileName = "Car Movement Data", menuName = "Lost Resort/Cars/Car Movement Data")]
    public class CarMovementData : ScriptableObject
    {
        [Header("Acceleration Settings")]
        [SerializeField] private float maxSpeed = 300f;

        public float MaxSpeed => maxSpeed;
    }
}
