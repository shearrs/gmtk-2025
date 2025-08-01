using UnityEngine;

namespace LostResort.Cameras
{
    [DefaultExecutionOrder(1000)]
    public class CarCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform target;

        [Header("Positioning")]
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 lookAtOffset;
        [SerializeField] private float smoothTime = 25f;

        [Header("Occlusion")]
        [SerializeField] private LayerMask occlusionLayers;
        [SerializeField] private float occlusionRadius = 0.5f;
        [SerializeField] private float occlusionPadding = 0.1f;

        private Vector3 targetPosition;
        private Vector3 occludedPosition;
        private bool isOccluded;
        private Vector3 currentVel;

        private Vector3 Offset
        {
            get
            {
                var off = target.TransformPoint(offset);
                off.y = target.position.y + offset.y;

                return off;
            }
        }
        private Vector3 LookAtOffset
        {
            get
            {
                var offset = target.TransformPoint(lookAtOffset);
                offset.y = target.position.y + lookAtOffset.y;

                return offset;
            }
        }

        private void OnValidate()
        {
            if (target == null)
                return;

            Vector3 forward = (LookAtOffset - transform.position).normalized;
            var rotation = Quaternion.LookRotation(forward);

            transform.SetPositionAndRotation(Offset, rotation);
        }

        private void Awake()
        {
            targetPosition = transform.position;
            occludedPosition = transform.position;
        }

        private void LateUpdate()
        {
            UpdateTargetPosition();
            UpdatePosition();
            UpdateRotation();
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
                return;
            }

            isOccluded = true;

            float targetDistance = hit.distance - occlusionPadding;
            Vector3 targetOccludedPos = LookAtOffset + direction * targetDistance;

            occludedPosition = targetOccludedPos;
        }

        private void UpdateTargetPosition()
        {
            targetPosition = Offset;
        }

        private void UpdateRotation()
        {
            var rotation = Quaternion.LookRotation(LookAtOffset - transform.position, Vector3.up);

            transform.rotation = rotation;
        }

        private void UpdatePosition()
        {
            Vector3 targetPos = isOccluded ? occludedPosition : targetPosition;

            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVel, smoothTime);
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
