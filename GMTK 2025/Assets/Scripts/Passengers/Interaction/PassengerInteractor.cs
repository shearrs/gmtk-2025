using LostResort.Interaction;
using LostResort.SignalShuttles;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerInteractor : Interactor<PassengerInteractable>
    {
        [SerializeField] private PassengerStorage passengerStorage;

        protected override void InteractWithType(PassengerInteractable interactable)
        {
            SignalShuttle.Emit(new InteractableAudioTriggeredSignal());
            passengerStorage.AddPassenger(interactable.Passenger);
        }
    }

    public struct InteractableAudioTriggeredSignal : ISignal
    {
        
    }
}
