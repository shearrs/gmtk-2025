using LostResort.Passengers;
using Shears;
using Shears.Input;
using Shears.Tweens;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LostResort.Cars
{
    public class CarRespawner : MonoBehaviour
    {
        [SerializeField] private Car car;
        [SerializeField] private PassengerStorage passengerStorage;
        [SerializeField] private Canvas respawnCanvas;
        [SerializeField] private Image background;

        private bool isRespawning = false;

        public Vector3 RespawnLocation { get; set; }

        private void OnEnable()
        {
            car.Input.ResetInput.Performed += OnResetInput;
        }

        private void OnDisable()
        {
            car.Input.ResetInput.Performed -= OnResetInput;
        }

        private void Start()
        {
            RespawnLocation = car.transform.position;
        }

        private void OnResetInput(ManagedInputInfo info)
        {
            Debug.Log("respawn");
            Respawn();
        }

        public void Respawn()
        {
            if (isRespawning)
                return;

            StartCoroutine(IERespawn());
        }

        private IEnumerator IERespawn()
        {
            isRespawning = true;

            car.Rigidbody.isKinematic = true;
            car.Disable();
            passengerStorage.ClearPassengers();
            background.color = Color.clear;
            respawnCanvas.enabled = true;
            var tween = background.DoColorTween(Color.midnightBlue);

            while (tween.IsPlaying)
                yield return null;

            car.Rigidbody.position = RespawnLocation;

            yield return CoroutineUtil.WaitForSeconds(1.5f);

            tween = background.DoColorTween(Color.clear);

            while (tween.IsPlaying)
                yield return null;

            respawnCanvas.enabled = false;
            car.Rigidbody.isKinematic = false;
            car.Enable();
            isRespawning = false;
        }
    }
}
