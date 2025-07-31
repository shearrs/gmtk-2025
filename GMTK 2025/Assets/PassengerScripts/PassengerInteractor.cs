using System.Collections.Generic;
using UnityEngine;

public class PassengerInteractor : MonoBehaviour
{
    private List<Passenger> passengers = new List<Passenger>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Passenger"))
        {
            Passenger passenger = other.gameObject.GetComponent<Passenger>();
            PickUpPassenger(passenger);     
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Location"))
        {
            ResortLocation location = other.gameObject.GetComponent<ResortLocation>();
            LetPassengersOutAtLocation(location.GetLocationType());
        }
    }
    
    private void AddPassenger(Passenger newPassenger)
    {
        passengers.Add(newPassenger);
    }

    private void PickUpPassenger(Passenger passenger)
    {
        passenger.PickUp();
        AddPassenger(passenger);
    }

    /// <summary>
    /// Runs through each passenger, sees if they match the dropOffLocation provided. If they do, drop off that player. 
    /// </summary>
    /// <param name="dropOffLocation">A locationType which is provided by the player when this function is called which represents the location players are being dropped off at.</param>
    private void LetPassengersOutAtLocation(LocationType dropOffLocation)
    {
        for (int i = passengers.Count - 1; i >= 0; i--)
        {
            if (passengers[i].passengerData.dropOffLocation == dropOffLocation)
            {
                passengers[i].DropOff();

                passengers.RemoveAt(i);
                
            }
        }
    }
}
