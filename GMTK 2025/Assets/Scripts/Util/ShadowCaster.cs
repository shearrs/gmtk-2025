using UnityEngine;

public class ShadowCaster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private GameObject shadow;

    [Header("Settings")]
    [SerializeField] private Vector3 shadowOrigin = Vector3.zero;
    [SerializeField] private float maxShadowDistance = 5f;
    [SerializeField] private Vector3 smallShadowScale = new(0.1f, 0.1f, 0.1f);
    [SerializeField] private Vector3 largeShadowScale = new(1f, 1f, 1f);
    [SerializeField] private float shadowFloorOffset = 0.01f;

    private void Update()
    {
        UpdateShadow();
    }

    private void UpdateShadow()
    {
        if (!Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, maxShadowDistance, collisionMask))
        {
            shadow.SetActive(false);
            return;
        }

        shadow.SetActive(true);

        Vector3 origin = transform.TransformPoint(shadowOrigin);
        float sqrOriginDistance = (origin - transform.position).sqrMagnitude;
        float distanceToGround = hit.distance;
        Vector3 shadowPos;

        if (distanceToGround * distanceToGround <= sqrOriginDistance)
            shadowPos = origin;
        else
        {
            shadowPos = hit.point;
            shadowPos.y += shadowFloorOffset;
        }

        float distanceT = (shadowPos - origin).sqrMagnitude / (maxShadowDistance * maxShadowDistance);
        Vector3 scale = Vector3.Lerp(smallShadowScale, largeShadowScale, distanceT);

        shadow.transform.position = shadowPos;
        shadow.transform.localScale = scale;
        shadow.transform.up = hit.normal;

        var angle = Vector3.SignedAngle(shadow.transform.forward, transform.forward, Vector3.up);
        shadow.transform.Rotate(Vector3.up, angle);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(transform.TransformPoint(shadowOrigin), 0.1f);
    }
}
