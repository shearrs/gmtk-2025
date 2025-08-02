using LostResort.Interaction;
using UnityEngine;

namespace LostResort.Passengers
{
    public class LocationInteractor : Interactor<LocationInteractable>
    {
        [SerializeField] private PassengerStorage passengerStorage;

        protected override void InteractWithType(LocationInteractable location)
        {
            passengerStorage.DropoffAtLocation(location.Location);
        }
    }
}
