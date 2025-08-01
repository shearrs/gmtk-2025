using UnityEngine;

namespace LostResort.Cameras
{
    public class CarCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody target;

        [Header("Positioning")]
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 lookAtOffset;
        [SerializeField] private float smoothTime = 25f;

        [Header("Occlusion")]
        [SerializeField] private LayerMask occlusionLayers;
        [SerializeField] private float occlusionRadius = 0.5f;
        [SerializeField] private float occlusionPadding = 0.1f;
        [SerializeField] private float occlusionFixTime = 0.1f;

        private Vector3 targetPosition;
        private Vector3 occludedPosition;
        private bool isOccluded;
        private Vector3 currentVel;
        private Vector3 currentOccludedVel;

        private Vector3 Offset => target.transform.TransformPoint(offset);
        private Vector3 LookAtOffset => target.transform.TransformPoint(lookAtOffset);

        private void OnValidate()
        {
            if (target == null)
                return;

            Vector3 forward = (LookAtOffset - transform.position).normalized;
            var rotation = Quaternion.LookRotation(forward);

            transform.SetPositionAndRotation(Offset, rotation);
        }

        private void LateUpdate()
        {
            UpdateTargetPosition();
            UpdateRotation();
            UpdatePosition();
        }

        private void FixedUpdate()
        {
            UpdateOcclusion();
        }

        private void UpdateOcclusion()
        {
            Vector3 heading = (targetPosition - LookAtOffset);
            float distance = heading.magnitude;
            Vector3 direction = heading.normalized;

            if (!Physics.SphereCast(LookAtOffset, occlusionRadius, direction, out var hit, distance, occlusionLayers))
            {
                isOccluded = false;
                currentOccludedVel = Vector3.zero;
                return;
            }

            isOccluded = true;

            float targetDistance = hit.distance - occlusionPadding;
            Vector3 targetOccludedPos = LookAtOffset + direction * targetDistance;

            occludedPosition = Vector3.SmoothDamp(transform.position, targetOccludedPos, ref currentOccludedVel, occlusionFixTime);
        }

        private void UpdateTargetPosition()
        {
            targetPosition = Vector3.SmoothDamp(targetPosition, Offset, ref currentVel, smoothTime);
        }

        private void UpdateRotation()
        {
            var rotation = Quaternion.LookRotation(LookAtOffset - transform.position, Vector3.up);

            transform.rotation = rotation;
        }

        private void UpdatePosition()
        {
            var targetPos = isOccluded ? occludedPosition : targetPosition;

            transform.position = targetPos;
        }

        private void OnDrawGizmosSelected()
        {
            if (target == null)
                return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(Offset, occlusionRadius);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(LookAtOffset, 0.5f);
        }
    }
}
