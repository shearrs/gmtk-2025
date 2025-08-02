using System;
using UnityEngine;

namespace LostResort.Interaction
{
    public class Interactable : MonoBehaviour, IInteractable
    {
        public event Action Interacted;

        public void Interact(Interactor interactor)
        {
            Interacted?.Invoke();
        }
    }
}