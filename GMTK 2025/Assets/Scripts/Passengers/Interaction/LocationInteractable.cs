using LostResort.Interaction;
using UnityEngine;

namespace LostResort.Passengers
{
    public class LocationInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private ResortLocation location;

        public ResortLocation Location => location;

        public void Interact(Interactor interactor)
        {
            interactor.InteractWith(this);
        }
    }
}
