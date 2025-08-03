using UnityEngine;

namespace LostResort.Interaction
{
    public abstract class Interactor : MonoBehaviour
    {
        public abstract void InteractWith(IInteractable interactable);

        public abstract bool CanInteractWith(IInteractable interactable);
    }

    public abstract class Interactor<T> : Interactor where T : IInteractable
    {
        public override void InteractWith(IInteractable interactable)
        {
            InteractWithType((T)interactable);
        }

        protected abstract void InteractWithType(T interactable);

        public override bool CanInteractWith(IInteractable interactable)
        {
            return interactable.GetType() == typeof(T);
        }
    }
}
