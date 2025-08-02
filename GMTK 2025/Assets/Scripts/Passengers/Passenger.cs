using LostResort.Score;
using LostResort.SignalShuttles;
using UnityEngine;
using UnityEngine.AI;

namespace LostResort.Passengers
{
    public class Passenger : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private Rigidbody rb;

        [Header("Settings")]
        [SerializeField] private float waitingTime = 30.0f;
        [SerializeField] private int scoreValue = 100;
        private ResortLocation originLocation;
        private ResortLocation targetLocation;

        public static Passenger Create(Passenger prefab, ResortLocation originLocation, ResortLocation targetLocation)
        {
            var passenger = Instantiate(prefab, originLocation.GetPickupPosition(), Quaternion.identity);
            passenger.originLocation = originLocation;
            passenger.targetLocation = targetLocation;

            passenger.agent.SetDestination(originLocation.GetPickupPosition());

            return passenger;
        }

        public void Pickup()
        {
            agent.enabled = false;
        }

        public void Dropoff()
        {
            SignalShuttle.Emit(new AddScoreSignal(scoreValue));
            Destroy(gameObject);
        }
    }
}
