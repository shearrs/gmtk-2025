using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerKiller : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var passenger = other.GetComponentInParent<Passenger>();

            if (passenger == null)
                return;

            passenger.Die();
        }
    }
}
