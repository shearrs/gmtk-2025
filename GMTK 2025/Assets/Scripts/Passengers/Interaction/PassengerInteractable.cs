using LostResort.Interaction;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private Passenger passenger;

        public Passenger Passenger => passenger;

        public void Interact(Interactor interactor)
        {
            if (!Passenger.IsPickedUp)
                interactor.InteractWith(this);
        }
    }
}
