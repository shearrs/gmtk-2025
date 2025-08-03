using UnityEngine;

namespace LostResort.Cars
{
    public class RespawnTrigger : MonoBehaviour
    {
        [SerializeField] private Transform respawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            var car = other.GetComponentInParent<Car>();

            if (car == null)
                return;

            var respawner = car.GetComponentInChildren<CarRespawner>();
            respawner.RespawnLocation = respawnPoint.position;
        }
    }
}
