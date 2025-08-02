using LostResort.Interaction;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerInteractor : Interactor<PassengerInteractable>
    {
        [SerializeField] private PassengerStorage passengerStorage;

        protected override void InteractWithType(PassengerInteractable interactable)
        {
            Debug.Log("Passenger interact");
            passengerStorage.AddPassenger(interactable.Passenger);
        }
    }
}
