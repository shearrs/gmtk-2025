using LostResort.Interaction;
using LostResort.Levels;
using LostResort.Passengers;
using LostResort.SignalShuttles;
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
        SignalShuttle.Emit(new InteractableAudioTriggeredSignal());

        foreach (var gate in gates)
            gate.ToggleOpen();
    }
}
