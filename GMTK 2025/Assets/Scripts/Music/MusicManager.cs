using LostResort.SignalShuttles;
using System.Collections;
using UnityEngine;

namespace LostResort.Music
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioSource hotelMusicSource;
        [SerializeField] private AudioSource beachMusicSource;
        [SerializeField] private AudioSource gymMusicSource;
        [SerializeField] private float musicVolume;
        [SerializeField] private float switchTime;

        private AudioSource fadingTarget;
        private AudioSource current;

        private void OnEnable()
        {
            SignalShuttle.Register<MusicChangeSignal>(OnMusicChangeSignal);
        }

        private void OnDisable()
        {
            SignalShuttle.Deregister<MusicChangeSignal>(OnMusicChangeSignal);
        }

        private void Awake()
        {
            current = hotelMusicSource;

            current.volume = musicVolume;
            beachMusicSource.volume = 0.0f;
            gymMusicSource.volume = 0.0f;
        }

        private void OnMusicChangeSignal(MusicChangeSignal signal)
        {
            AudioSource next = null;

            switch (signal.Type)
            {
                case MusicChangeSignal.MusicType.Hotel:
                    next = hotelMusicSource;
                    break;
                case MusicChangeSignal.MusicType.Beach:
                    next = beachMusicSource;
                    break;
                case MusicChangeSignal.MusicType.Gym:
                    next = gymMusicSource;
                    break;
            }

            if (next == current)
                return;

            SwitchTrack(current, next);
        }

        private void SwitchTrack(AudioSource current, AudioSource next)
        {
            StopAllCoroutines();

            if (fadingTarget != null)
            {
                current.volume = 0.0f;
                current = fadingTarget;
            }

            StartCoroutine(IESwitchTrack(current, next));
        }

        private IEnumerator IESwitchTrack(AudioSource current, AudioSource next)
        {
            float elapsedTime = 0.0f;
            float startVolume = current.volume;
            fadingTarget = next;

            while (elapsedTime < switchTime)
            {
                float t = elapsedTime / switchTime;

                current.volume = Mathf.Lerp(startVolume, 0.0f, t);
                next.volume = Mathf.Lerp(0.0f, musicVolume, t);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            this.current = next;
            fadingTarget = null;
        }
    }
}