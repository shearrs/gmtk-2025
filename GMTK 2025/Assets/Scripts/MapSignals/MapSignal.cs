using System;
using UnityEngine;
using LostResort.SignalShuttles;

public struct RegionChange : ISignal
{
    public enum Regions
    {
        Hotel, ConferenceCenter, Beach, Gym 
    }
    
    public Regions NewRegion { get; private set; }

    public RegionChange(Regions newRegion)
    {
        NewRegion = newRegion;
    }
}

namespace LostResort.MapSignals
{
    public class MapSignal : MonoBehaviour
    {
        [SerializeField] private Collider[] _regionEntryTriggers;

        private RegionChange.Regions _currentRegion;

        private void OnEnable()
        {
            RegisterSignals();
        }

        private void OnDisable()
        {
            DeregisterSignals();
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("penis");
            //Check if trigger was a region entry trigger
            if(!Array.Exists(_regionEntryTriggers, x => x.gameObject == other.gameObject))
            {
                return;
            }
            
            int index = Array.FindIndex(_regionEntryTriggers, x => x.gameObject == other.gameObject);

            if ((int)_currentRegion == index)
            {
                return;
            }

            _currentRegion = (RegionChange.Regions)index;
            SignalShuttle.Emit(new RegionChange(_currentRegion));
        }
        
        void OnGameStart(OnGameStart signal)
        {
            _currentRegion = RegionChange.Regions.Hotel;
            SignalShuttle.Emit(new RegionChange(_currentRegion));
        }

        void RegisterSignals()
        {
            SignalShuttle.Register<OnGameStart>(OnGameStart);
        }
        
        void DeregisterSignals()
        {
            SignalShuttle.Deregister<OnGameStart>(OnGameStart);
        }
    }
}