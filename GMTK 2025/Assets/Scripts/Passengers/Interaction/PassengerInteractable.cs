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
            //Debug.Log("interacted with: picked up: " + Passenger.IsPickedUp + " alive: " + Passenger.IsAlive);

            if (!Passenger.IsPickedUp && Passenger.IsAlive)
                interactor.InteractWith(this);
        }
    }
}
