using Shears;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Passenger passengerPrefab;
        [SerializeField] private ResortLocation[] locations;

        [Header("Settings")]
        [SerializeField] private float spawnTime;

        private readonly List<ResortLocation> instanceLocations = new();

        private void Start()
        {
            StartCoroutine(IESpawn());
        }

        private IEnumerator IESpawn()
        {
            while (true)
            {
                yield return CoroutineUtil.WaitForSeconds(spawnTime);

                var originLocation = GetOriginLocation();
                var targetLocation = GetTargetLocation(originLocation);
                Passenger.Create(passengerPrefab, originLocation, targetLocation);
            }
        }

        private ResortLocation GetOriginLocation()
        {
            int random = Random.Range(0, locations.Length);

            return locations[random];
        }

        private ResortLocation GetTargetLocation(ResortLocation origin)
        {
            instanceLocations.Clear();
            instanceLocations.AddRange(locations);
            instanceLocations.Remove(origin);

            int random = Random.Range(0, instanceLocations.Count);

            return instanceLocations[random];
        }
    }
}
