using Shears;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostResort.Passengers
{
    public class PassengerSpawner : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Passenger malePassengerPrefab;
        [SerializeField] private Passenger femalePassengerPrefab;
        [SerializeField] private ResortLocation[] locations;

        [Header("Settings")]
        [SerializeField] private float spawnTime;

        private readonly List<ResortLocation> instanceLocations = new();

        private void Start()
        {
            StartCoroutine(IESpawn());
        }

        private void Update()
        {
            Physics.SyncTransforms();
        }

        private IEnumerator IESpawn()
        {
            while (true)
            {
                yield return CoroutineUtil.WaitForSeconds(spawnTime);

                var originLocation = GetOriginLocation();
                var targetLocation = GetTargetLocation(originLocation);
                int random = Random.Range(0, 2);
                var prefab = (random == 0) ? malePassengerPrefab : femalePassengerPrefab;

                Passenger.Create(prefab, originLocation, targetLocation);
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
