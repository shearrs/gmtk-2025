using LostResort.Interaction;
using LostResort.Levels;
using UnityEngine;

public class PowerPole : MonoBehaviour
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private Gate[] gates;

    private void OnEnable()
    {
        interactable.Interacted += OnInteracted;
    }

    private void OnDisable()
    {
        interactable.Interacted -= OnInteracted;
    }

    private void OnInteracted()
    {
        foreach (var gate in gates)
            gate.ToggleOpen();
    }
}
