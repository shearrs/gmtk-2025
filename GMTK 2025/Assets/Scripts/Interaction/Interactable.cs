using System;
using UnityEngine;

namespace LostResort.Interaction
{
    public class Interactable : MonoBehaviour
    {
        public event Action Interacted;

        public void Interact()
        {
            Debug.Log("interacted!");
            Interacted?.Invoke();
        }
    }
}