using System;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerKiller : MonoBehaviour
    {
        public event Action KilledSomeone;

        private void OnTriggerEnter(Collider other)
        {
            var passenger = other.GetComponentInParent<Passenger>();

            if (passenger == null)
                return;

            KilledSomeone?.Invoke();
            passenger.Die();
        }
    }
}
