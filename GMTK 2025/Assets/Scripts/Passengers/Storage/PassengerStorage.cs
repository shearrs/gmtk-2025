using System.Collections.Generic;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerStorage : MonoBehaviour
    {
        [SerializeField] private PassengerSlot[] slots = new PassengerSlot[1];

        private readonly List<PassengerSlot> openSlots = new();
        private readonly List<PassengerSlot> reservedSlots = new();

        private void Awake()
        {
            openSlots.AddRange(slots);
        }

        public void AddPassenger(Passenger passenger)
        {
            if (openSlots.Count == 0)
                return;

            var slot = openSlots[0];

            openSlots.RemoveAt(0);
            reservedSlots.Add(slot);

            passenger.OnPickup();
            slot.SetPassenger(passenger);
        }

        public void RemovePassenger(Passenger passenger)
        {
            PassengerSlot targetSlot = null;

            foreach (var slot in reservedSlots)
            {
                if (slot.Passenger == passenger)
                {
                    targetSlot = slot;
                    break;
                }
            }

            if (targetSlot == null)
                return;

            targetSlot.ClearPassenger();
            reservedSlots.Remove(targetSlot);
            openSlots.Add(targetSlot);

            passenger.OnDropoff();
        }
    }
}
