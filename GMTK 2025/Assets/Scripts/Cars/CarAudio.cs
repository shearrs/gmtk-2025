using System.Collections;
using System.Collections.Generic;
using LostResort.Interaction;
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
        
        [Header("Clips")]
        
        [SerializeField]
        private AudioSource loopDrift;
        [SerializeField]
        private AudioSource passengerDeath;
        [SerializeField] 
        private AudioSource carSound;
        [SerializeField] 
        private AudioSource twoBeeps;

        
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
            passengerKiller.KilledSomeone += OnKilledSomeone;
            SignalShuttle.Register<InteractableAudioTriggeredSignal>(OnInteracted);


        }

        private void OnDisable()
        {
            driftController.BeganDrifting -= OnBeganDrifting;
            driftController.EndedDrifting -= OnEndedDrifting;
            passengerKiller.KilledSomeone -= OnKilledSomeone;
            SignalShuttle.Deregister<InteractableAudioTriggeredSignal>(OnInteracted);


        }

        private void OnInteracted(InteractableAudioTriggeredSignal interactableAudioTriggered)
        {
            //remember to make this happen when the player goes around a interactable pole too
            twoBeeps.Play();
        }

        private void FixedUpdate()
        {
            if (accelerationInput > 0.1f)
            {
                if (!carSound.isPlaying)
                    FadeIn(carSound, carFadeIn);
            }
            else
            {
                FadeOut(carSound, carFadeOut);
            }
        }

        private void OnKilledSomeone()
        {
            passengerDeath.Play();
        }

        private void OnBeganDrifting()
        {
            FadeIn(loopDrift, driftFadeIn);
        }

        private void OnEndedDrifting()
        {
            FadeOut(loopDrift, driftFadeOut);
        }
     
        private void FadeIn(AudioSource audioSource, float fadeDuration)
        {
            StartCoroutine(FadeAudio(0f, 1f, audioSource, fadeDuration));
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
