using UnityEngine;

namespace LostResort.Passengers
{
    public class Accessory : MonoBehaviour
    {
        public enum AccessoryType { Hat, Face, Neck, Hand, Wrist, Waist, Feet }

        [SerializeField] private AccessoryType type;

        public AccessoryType Type => type;
    }
}
