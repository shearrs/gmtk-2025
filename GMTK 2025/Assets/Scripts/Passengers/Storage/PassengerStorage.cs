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
        private readonly List<Passenger> secondInstancePassengers = new();

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
            
            SignalShuttle.Emit(new PassengersChangedSignal(GetPassengers()));
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

            SignalShuttle.Emit(new PassengersChangedSignal(GetPassengers()));
            targetSlot.ClearPassenger();
            reservedSlots.Remove(targetSlot);
            openSlots.Add(targetSlot);

            passenger.OnDropoff();
        }

        public void ClearPassengers()
        {
            foreach (var slot in reservedSlots)
            {
                slot.Passenger.FullyDestroy();
                slot.ClearPassenger();
            }

            openSlots.AddRange(reservedSlots);
            reservedSlots.Clear();

            SignalShuttle.Emit(new PassengersChangedSignal(GetPassengers()));
        }

        public void DropoffAtLocation(ResortLocation location)
        {
            instancePassengers.Clear();

            foreach (var slot in reservedSlots)
            {
                if (slot.Passenger.TargetLocation == location)
                    instancePassengers.Add(slot.Passenger);
            }

            secondInstancePassengers.Clear();
            secondInstancePassengers.AddRange(instancePassengers);

            foreach (var passenger in secondInstancePassengers)
                RemovePassenger(passenger);
        }

        private List<Passenger> GetPassengers()
        {
            instancePassengers.Clear();

            foreach (var slot in reservedSlots)
            {
                instancePassengers.Add(slot.Passenger);
            }

            return instancePassengers;
        }
    }
}
