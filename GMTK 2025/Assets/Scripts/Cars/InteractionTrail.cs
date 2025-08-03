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
        [SerializeField] private Interactor[] interactors;

        [Header("Settings")]
        [SerializeField] private float testRadius = 0.5f;
        [SerializeField] private LayerMask interactionLayer;

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
            Vector3 rotation = trailRenderer.transform.eulerAngles;
            rotation.x = 90;
            trailRenderer.transform.eulerAngles = rotation;

            if (isDrifting)
            {
                TestForExitingTestPosition();
                TestForLoop();
            }
        }

        private void OnBeganDrifting()
        {
            trailRenderer.Clear();
            testPositions.Clear();
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
                    InteractWithLoop();

                    createdLoop = true;
                    break;
                }
            }

            if (createdLoop)
            {
                testPositions.Clear();
                trailRenderer.Clear();
                UpdateTestPosition();
            }
        }

        private void InteractWithLoop()
        {
            Vector3 center = Vector3.zero;

            foreach (var position in testPositions)
                center += position;

            center /= testPositions.Count;

            var furthestPoint = GetFurthestPoint(center);
            float radius = 0.85f * (furthestPoint - center).magnitude;

            int hits = Physics.OverlapSphereNonAlloc(center, radius, overlapColliders, interactionLayer, QueryTriggerInteraction.Collide);

            Debug.DrawLine(center, furthestPoint, Color.red, 1000f);

            for (int i = 0; i < hits; i++)
            {
                var hit = overlapColliders[i];

                if (hit.TryGetComponent(out IInteractable interactable))
                    TryInteract(interactable);
            }
        }

        private void TryInteract(IInteractable interactable)
        {
            foreach (var interactor in interactors)
            {
                if (interactor.CanInteractWith(interactable))
                {
                    interactable.Interact(interactor);
                    return;
                }
            }

            interactable.Interact(null);
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
