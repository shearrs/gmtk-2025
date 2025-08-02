using LostResort.Interaction;
using LostResort.Score;
using LostResort.SignalShuttles;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LostResort.Passengers
{
    public class Passenger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Collider col;

        [Header("Settings")]
        [SerializeField] private float waitingTime = 30.0f;
        [SerializeField] private int scoreValue = 100;

        private ResortLocation originLocation;
        private ResortLocation targetLocation;

        public bool IsPickedUp { get; private set; } = false;

        public static Passenger Create(Passenger prefab, ResortLocation originLocation, ResortLocation targetLocation)
        {
            var passenger = Instantiate(prefab, originLocation.GetPickupPosition(), Quaternion.identity);
            passenger.originLocation = originLocation;
            passenger.targetLocation = targetLocation;

            passenger.agent.SetDestination(originLocation.GetPickupPosition());

            return passenger;
        }

        public void SetAnchor(FixedJoint joint)
        {
            rb.position = joint.transform.position;
            rb.rotation = joint.transform.rotation;
            rb.PublishTransform();
            joint.connectedBody = rb;
        }

        public void OnPickup()
        {
            IsPickedUp = true;
            agent.enabled = false;
            rb.useGravity = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            col.enabled = false;
        }

        public void OnDropoff()
        {
            IsPickedUp = false;
            SignalShuttle.Emit(new AddScoreSignal(scoreValue));
            Destroy(gameObject);
        }
    }
}
