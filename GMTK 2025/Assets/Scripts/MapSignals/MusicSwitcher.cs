using System;
using UnityEngine;
using LostResort.SignalShuttles;

namespace LostResort.MapSignals
{
    public class MusicSwitcher : MonoBehaviour
    {
        [SerializeField] private AudioSource[] _musicTracks = new AudioSource[3];

        private void OnEnable()
        {
            RegisterSignals();
        }

        private void OnDisable()
        {
            DeregisterSignals();
        }

        void SwitchTrack(int newTrack)
        {
            //Debug.Log(newTrack);
            if (newTrack >= _musicTracks.Length)
            {
                return;
            }

            for (int i = 0; i < _musicTracks.Length; i++)
            {
                if (i == newTrack)
                {
                    _musicTracks[i].volume = 1f;
                    continue;
                }

                _musicTracks[i].volume = 0f;
            }
        }

        void OnRegionChange(RegionChange signal)
        {
            SwitchTrack((int)signal.NewRegion);
        }

        void RegisterSignals()
        {
            SignalShuttle.Register<RegionChange>(OnRegionChange);
        }

        void DeregisterSignals()
        {
            SignalShuttle.Deregister<RegionChange>(OnRegionChange);
        }
    }
}
