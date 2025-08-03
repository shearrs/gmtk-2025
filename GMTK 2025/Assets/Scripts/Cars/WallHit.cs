using System;
using LostResort.Passengers;
using LostResort.SignalShuttles;
using UnityEngine;

namespace LostResort.Cars
{
    public class WallHit : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger)
            {
                return;
            }
            //Debug.Log(other.gameObject.name);
            var passenger = other.GetComponentInParent<Passenger>();

            if (passenger != null)
                return;

            SignalShuttle.Emit(new HitSomethingSignal());
        }
    }
}