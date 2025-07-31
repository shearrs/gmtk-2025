using UnityEngine;

public class Passenger : MonoBehaviour
{
    public PassengerData passengerData { get; private set; }
    private bool inShuttle;
    private MeshRenderer meshRenderer;
    private CapsuleCollider capsuleCollider;
    
    
    [SerializeField] private Material[] dropOffLocationBasedMaterials;
    
    
    
    private void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void InitializeLocation(LocationType locationType)
    {
        passengerData = new PassengerData(locationType);
        meshRenderer.material = dropOffLocationBasedMaterials[(int)passengerData.dropOffLocation];
    }

    /// <summary>
    /// Picks up the passenger. This is called from some player behavior script. The passenger should be saved in a list of passengers which is stored in the player.
    /// This script, or where its called, should hide the passenger (or have them join the shuttle).
    /// It also changed the boolean 'inShuttle' to false.
    /// </summary>
    public void PickUp() {
        Debug.Log($"Picked up a player from {passengerData.startingLocation} who intends to travel to {passengerData.dropOffLocation}!");
        meshRenderer.enabled = false;
        capsuleCollider.enabled = false;
        inShuttle = true;
    }
    
    
    public void DropOff() {
        Debug.Log($"Dropped off a player at {passengerData.dropOffLocation} who originally came from {passengerData.startingLocation}!");
        inShuttle = false;
        Destroy(gameObject);
        
        //increase score
    }


}
