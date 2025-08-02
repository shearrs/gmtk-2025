using System.Collections.Generic;
using UnityEngine;

namespace LostResort.Passengers
{
    public class ResortLocation : MonoBehaviour
    {
        [SerializeField] private Vector3 pickupPosition;
        [SerializeField] private Vector3 dropoffPosition;
        [SerializeField] private List<Accessory> accessories;

        public IReadOnlyList<Accessory> Accessories => accessories;

        public Vector3 GetPickupPosition() => transform.TransformPoint(pickupPosition);
        public Vector3 GetDropoffPosition() => transform.TransformPoint(dropoffPosition);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(pickupPosition), 1.0f);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(dropoffPosition), 1.0f);
        }
    }
}
