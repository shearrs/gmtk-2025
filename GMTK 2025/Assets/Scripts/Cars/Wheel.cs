using UnityEngine;

namespace LostResort.Cars
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private Rigidbody carRigidBody;

        [Header("Collision Settings")]
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private float raycastDistance = 0.5f;

        [Header("Suspension Settings")]
        [SerializeField] private float springRestDistance = 0.5f;
        [SerializeField] private float springStrength = 5000f;
        [SerializeField] private float springDamping = 100f;

        private void FixedUpdate()
        {
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, raycastDistance, collisionLayer))
            {
                Debug.Log("hit");
                ApplySuspension(hit);
            }
        }

        private void ApplySuspension(RaycastHit hit)
        {
            Vector3 springDirection = transform.up;
            Vector3 tireWorldVel = carRigidBody.GetPointVelocity(transform.position);

            float offset = springRestDistance - hit.distance;
            float velocity = Vector3.Dot(springDirection, tireWorldVel);
            float force = (offset * springStrength) - (velocity * springDamping);

            carRigidBody.AddForceAtPosition(springDirection * force, transform.position);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, -transform.up * raycastDistance);
        }
    }
}
