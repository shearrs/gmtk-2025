using System.Collections.Generic;
using LostResort.SignalShuttles;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerStorage : MonoBehaviour
    {
        [SerializeField] private PassengerSlot[] slots = new PassengerSlot[1];

        private readonly List<PassengerSlot> openSlots = new();
        private readonly List<PassengerSlot> reservedSlots = new();
        private readonly List<Passenger> instancePassengers = new();

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
            
            //Debug.Log(passenger.targetLocation.resortLocationName);
            SignalShuttle.Emit(new PassengersChangedSignal(true, passenger.targetLocation.resortLocationName));
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

            SignalShuttle.Emit(new PassengersChangedSignal(false, passenger.targetLocation.resortLocationName));
            targetSlot.ClearPassenger();
            reservedSlots.Remove(targetSlot);
            openSlots.Add(targetSlot);

            passenger.OnDropoff();
        }

        public void ClearPassengers()
        {
            foreach (var slot in reservedSlots)
            {
                Destroy(slot.Passenger);
                slot.ClearPassenger();
            }

            openSlots.AddRange(reservedSlots);
            reservedSlots.Clear();
        }

        public void DropoffAtLocation(ResortLocation location)
        {
            instancePassengers.Clear();

            foreach (var slot in reservedSlots)
            {
                if (slot.Passenger.TargetLocation == location)
                    instancePassengers.Add(slot.Passenger);
            }

            foreach (var passenger in instancePassengers)
                RemovePassenger(passenger);
        }
    }
}
