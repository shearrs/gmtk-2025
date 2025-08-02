using LostResort.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace LostResort.Cars
{
    public class InteractionTrail : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DriftController driftController;
        [SerializeField] private TrailRenderer trailRenderer;

        [Header("Settings")]
        [SerializeField] private float testRadius = 0.5f;

        private readonly Collider[] overlapColliders = new Collider[10];
        private readonly List<Vector3> testPositions = new();
        private bool isDrifting = false;
        private Vector3 currentTestPosition;

        private void OnEnable()
        {
            driftController.BeganDrifting += OnBeganDrifting;
            driftController.EndedDrifting += OnEndedDrifting;
        }

        private void OnDisable()
        {
            driftController.BeganDrifting -= OnBeganDrifting;
            driftController.EndedDrifting -= OnEndedDrifting;
        }

        private void Update()
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.x = 90;
            transform.eulerAngles = rotation;

            if (isDrifting)
            {
                TestForExitingTestPosition();
                TestForLoop();
            }
        }

        private void OnBeganDrifting()
        {
            testPositions.Add(transform.position);
            currentTestPosition = transform.position;
            trailRenderer.emitting = true;
            isDrifting = true;
        }

        private void OnEndedDrifting()
        {
            trailRenderer.emitting = false;
            trailRenderer.Clear();
            testPositions.Clear();
            isDrifting = false;
        }

        private void TestForExitingTestPosition()
        {
            float sqrDistance = (testPositions[^1] - driftController.transform.position).sqrMagnitude;
            float diameter = 2.0f * testRadius;

            if (sqrDistance > diameter * diameter)
            {
                UpdateTestPosition();
            }
        }

        private void TestForLoop()
        {
            bool createdLoop = false;

            foreach (var position in testPositions)
            {
                if (position == currentTestPosition)
                    continue;

                float sqrDistance = (position - driftController.transform.position).sqrMagnitude;

                if (sqrDistance < testRadius * testRadius)
                {
                    InteractWithLoop(position);

                    createdLoop = true;
                    break;
                }
            }

            if (createdLoop)
            {
                testPositions.Clear();
                UpdateTestPosition();
            }
        }

        private void InteractWithLoop(Vector3 testPos)
        {
            Vector3 center = Vector3.zero;

            foreach (var position in testPositions)
                center += position;

            center /= testPositions.Count;

            var furthestPoint = GetFurthestPoint(center);
            float radius = (furthestPoint - center).magnitude;

            int hits = Physics.OverlapSphereNonAlloc(center, radius, overlapColliders, -1, QueryTriggerInteraction.Collide);

            for (int i = 0; i < hits; i++)
            {
                var hit = overlapColliders[i];

                if (hit.TryGetComponent(out Interactable interactable))
                    interactable.Interact();
            }
        }

        private Vector3 GetFurthestPoint(Vector3 center)
        {
            Vector3 furthestPoint = Vector3.zero;
            float furthestDistance = 0.0f;

            for (int i = 0; i < trailRenderer.positionCount; i++)
            {
                var point = trailRenderer.GetPosition(i);

                float sqrDistance = (point - center).sqrMagnitude;

                if (sqrDistance > furthestDistance)
                {
                    furthestPoint = point;
                    furthestDistance = sqrDistance;
                }
            }

            return furthestPoint;
        }

        private void UpdateTestPosition()
        {
            testPositions.Add(driftController.transform.position);
            currentTestPosition = driftController.transform.position;
        }

        private void OnDrawGizmos()
        {
            foreach (var position in testPositions)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(position, testRadius);
            }
        }
    }
}
