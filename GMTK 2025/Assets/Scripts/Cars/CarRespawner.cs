using LostResort.Passengers;
using Shears;
using System.Collections;
using UnityEngine;

namespace LostResort.Cars
{
    public class CarRespawner : MonoBehaviour
    {
        [SerializeField] private Car car;
        [SerializeField] private PassengerStorage passengerStorage;
        [SerializeField] private Canvas respawnCanvas;

        public Vector3 RespawnLocation { get; set; }

        private void Start()
        {
            RespawnLocation = car.transform.position;
        }

        public void Respawn()
        {
            StartCoroutine(IERespawn());
        }

        private IEnumerator IERespawn()
        {
            car.Rigidbody.isKinematic = true;
            car.Input.Disable();
            passengerStorage.ClearPassengers();
            respawnCanvas.enabled = true;

            car.Rigidbody.position = RespawnLocation;

            yield return CoroutineUtil.WaitForSeconds(1.5f);

            respawnCanvas.enabled = false;
        }
    }
}
