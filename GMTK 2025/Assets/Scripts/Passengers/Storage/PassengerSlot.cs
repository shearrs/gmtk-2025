using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerSlot : MonoBehaviour
    {
        [SerializeField] private Passenger passenger;
        private Joint joint;

        public Passenger Passenger => passenger;

        private void Awake()
        {
            joint = GetComponent<Joint>();
        }

        public void SetPassenger(Passenger passenger)
        {
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
