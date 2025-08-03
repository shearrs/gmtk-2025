using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LostResort.Passengers
{
    public class ResortLocation : MonoBehaviour
    {
        
        [Header("Positions")]
        [SerializeField] private Vector3 pickupPosition;
        [SerializeField] private Vector3 dropoffPosition;
        [SerializeField] private float spawnRadius = 3f;

        [Header("Customization")]
        [SerializeField] private List<Accessory> accessories;
        [SerializeField] private List<Material> maleMaterials;
        [SerializeField] private List<Material> femaleMaterials;

        public enum ResortLocationName
        {
            Default,
            Conference,
            Hotel,
            Beach,
            Gym,
        }
        
        [field: SerializeField]
        public ResortLocationName resortLocationName {get; private set;}
        
        public IReadOnlyList<Accessory> Accessories => accessories;
        public IReadOnlyList<Material> MaleMaterials => maleMaterials;
        public IReadOnlyList<Material> FemaleMaterials => femaleMaterials;

        public Vector3 GetPickupPosition()
        {
            var pos = transform.TransformPoint(pickupPosition);

            Vector2 offset = Random.insideUnitCircle * spawnRadius;

            pos.x += offset.x;
            pos.z += offset.y;

            return pos;
        }
        public Vector3 GetDropoffPosition() => transform.TransformPoint(dropoffPosition);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(pickupPosition), 1.0f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(dropoffPosition), 1.0f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.TransformPoint(pickupPosition), spawnRadius);
        }
    }
}
