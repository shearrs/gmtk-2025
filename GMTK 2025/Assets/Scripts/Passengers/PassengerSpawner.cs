using System;
using System.Collections;
using LostResort.SignalShuttles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LostResort.Passengers
{
    public class PassengerSpawner : MonoBehaviour
    {
        [SerializeField] private float timeBetweenSpawns = 5f;
        [SerializeField] private GameObject passengerPrefab;
        [SerializeField] private BoxCollider[] boxColliders;

        
        void Start()
        {
            SignalShuttle.Emit(new OnGameStart());
            StartCoroutine(ContinuouslySpawnPassengers());
        }


        IEnumerator ContinuouslySpawnPassengers()
        {
            while (true)
            {
                RandomlySpawnPassenger();

                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }

        private void RandomlySpawnPassenger()
        {
            Array locations = Enum.GetValues(typeof(LocationType));
            int randomIndex = Random.Range(0, locations.Length);
            LocationType location = (LocationType)locations.GetValue(randomIndex);

            Vector3 randomPoint = GetRandomPointInBounds(boxColliders[randomIndex].bounds);
            Passenger passenger = Instantiate(passengerPrefab, randomPoint, Quaternion.identity, transform)
                .GetComponent<Passenger>();
            passenger.InitializeLocation(location);

        }
        
        Vector3 GetRandomPointInBounds(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }
    }
}