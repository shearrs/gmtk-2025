using System.Collections;
using LostResort.Passengers;
using LostResort.SignalShuttles;
using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    public class CarAudio : MonoBehaviour
    {
        [Header("Values")]
        [SerializeField]
        private float driftFadeIn;
        [SerializeField]
        private float driftFadeOut;
        
        [SerializeField]
        private float carFadeIn;
        [SerializeField]
        private float carFadeOut;

        [Header("Ranges")]
        [SerializeField]
        private Vector2 carLoopRange;

        [SerializeField]
        private Vector2 deathRange;

        [Header("Clips")]
        [SerializeField]
        private AudioSource loopDrift;
        [SerializeField]
        private AudioSource passengerDeath;
        [SerializeField] 
        private AudioSource carSound;
        [SerializeField] 
        private AudioSource twoBeeps;
        [SerializeField] 
        private AudioSource whoosh;

        [Header("References")]
        [SerializeField] 
        private DriftController driftController;

        [SerializeField]
        private PassengerKiller passengerKiller;
        
        [SerializeField]
        private Car car;

        private IManagedInput moveInput;
        private float accelerationInput = 0;
        
        private void Awake()
        {
            moveInput = car.Input.MoveInput;
        }

        private void Update()
        {
            accelerationInput = moveInput.ReadValue<Vector2>().y;
        }
        
        private void OnEnable()
        {
            driftController.BeganDrifting += OnBeganDrifting;
            driftController.EndedDrifting += OnEndedDrifting;
            driftController.PreformingWhoosh += OnWhoosh;

            passengerKiller.KilledSomeone += OnKilledSomeone;
            car.Input.MoveInput.Performed += OnAccelerationInput;
            SignalShuttle.Register<InteractableAudioTriggeredSignal>(OnInteracted);
        }

        private void OnDisable()
        {
            driftController.BeganDrifting -= OnBeganDrifting;
            driftController.EndedDrifting -= OnEndedDrifting;
            passengerKiller.KilledSomeone -= OnKilledSomeone;
            car.Input.MoveInput.Performed -= OnAccelerationInput;
            SignalShuttle.Deregister<InteractableAudioTriggeredSignal>(OnInteracted);
        }

        private void OnInteracted(InteractableAudioTriggeredSignal interactableAudioTriggered)
        {
            twoBeeps.Play();
        }

        private void OnAccelerationInput(ManagedInputInfo info)
        {
            carSound.pitch = Random.Range(carLoopRange.x, carLoopRange.y);
        }

        private void FixedUpdate()
        {
            if (Mathf.Abs(accelerationInput) > 0.1f)
            {
                if (!carSound.isPlaying)
                {
                    FadeIn(carSound, carFadeIn);
                }
            }
            else
                FadeOut(carSound, carFadeOut);
        }

        private void OnWhoosh()
        {
            whoosh.pitch = Random.Range(carLoopRange.x, carLoopRange.y);
            whoosh.Play();
        }

        private void OnKilledSomeone()
        {
            passengerDeath.pitch = Random.Range(deathRange.x, deathRange.y);
            passengerDeath.Play();
        }

        private void OnBeganDrifting()
        {
            loopDrift.pitch = Random.Range(carLoopRange.x, carLoopRange.y);
            FadeIn(loopDrift, driftFadeIn);
        }

        private void OnEndedDrifting()
        {
            
            FadeOut(loopDrift, driftFadeOut);
        }
     
        private void FadeIn(AudioSource audioSource, float fadeDuration)
        {
            float endVolume = 1f;


            StartCoroutine(FadeAudio(0f, endVolume, audioSource, fadeDuration));
        }

        private void FadeOut(AudioSource audioSource, float fadeDuration)
        {
            StartCoroutine(FadeAudio(audioSource.volume, 0f, audioSource, fadeDuration));
        }

        private IEnumerator FadeAudio(float startVolume, float endVolume, AudioSource audioSource, float fadeDuration)
        {
            float time = 0f;

            // If fading in, make sure the audio is playing first
            if (!audioSource.isPlaying && endVolume > startVolume)
                audioSource.Play();

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, endVolume, time / fadeDuration);
                yield return null;
            }

            audioSource.volume = endVolume;

            // If we just faded out, stop the audio
            if (endVolume == 0f)
                audioSource.Stop();
        }
    }
}
