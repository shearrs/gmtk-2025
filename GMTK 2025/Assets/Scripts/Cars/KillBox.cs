using UnityEngine;

namespace LostResort.Cars
{
    public class KillBox : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var car = other.GetComponentInParent<Car>();

            if (car == null)
                return;

            var respawner = car.GetComponentInChildren<CarRespawner>();

            respawner.Respawn();
        }
    }
}
