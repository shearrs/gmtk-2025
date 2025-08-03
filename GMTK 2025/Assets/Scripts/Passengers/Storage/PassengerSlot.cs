using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerSlot : MonoBehaviour
    {
        [SerializeField] private Passenger passenger;

        public Passenger Passenger => passenger;

        public void SetPassenger(Passenger passenger)
        {
            this.passenger = passenger;

            passenger.SetParent(transform);
            passenger.SetPosition(transform.position);
            passenger.SetRotation(transform.rotation);
        }

        public void ClearPassenger()
        {
            passenger = null;
        }
    }
}
