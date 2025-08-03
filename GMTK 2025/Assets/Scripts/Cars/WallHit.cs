using System;
using LostResort.Passengers;
using UnityEngine;

namespace LostResort.Cars
{
    public class WallHit : MonoBehaviour
    {
        public event Action HitAWall;

        private void OnTriggerEnter(Collider other)
        {
            var passenger = other.GetComponentInParent<Passenger>();

            if (passenger != null)
                return;

            HitAWall?.Invoke();
        }
    }
}
