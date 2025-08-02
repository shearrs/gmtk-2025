using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerSlot : MonoBehaviour
    {
        [SerializeField] private Passenger passenger;
        [SerializeField] private FixedJoint joint;

        public Passenger Passenger => passenger;

        public void SetPassenger(Passenger passenger)
        {
            this.passenger = passenger;

            passenger.SetAnchor(joint);
        }

        public void ClearPassenger()
        {
            passenger = null;
        }
    }
}
