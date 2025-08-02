using System;
using System.Collections.Generic;
using UnityEngine;
using LostResort.SignalShuttles;

public struct RegionChange : ISignal
{
    public enum Regions
    {
        Hotel, Beach, Gym
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
        /// <summary>
        /// Triggers must be indexed in the same order corresponding to RegionChange.Regions
        /// </summary>
        [SerializeField] private List<GameObject> _hotelEntryTriggers;
        
        [SerializeField] private List<GameObject> _beachEntryTriggers;
        
        [SerializeField] private List<GameObject> _gymEntryTriggers;

        private List<GameObject>[] _regionEntryTriggersArray;

        private RegionChange.Regions _currentRegion;

        private void OnEnable()
        {
            //RegisterSignals();
            MakeArray();
        }

        private void Start()
        {
            //TODO: comment this if you switch to using OnGameStart
            _currentRegion = RegionChange.Regions.Hotel;
            SignalShuttle.Emit(new RegionChange(_currentRegion));
        }

        private void OnDisable()
        {
            //DeregisterSignals();
        }

        private void OnTriggerEnter(Collider other)
        {
            //Check if trigger was a region entry trigger
            if(!Array.Exists(_regionEntryTriggersArray, x => x.Contains(other.gameObject)))
            {
                return;
            }
            
            int index = Array.FindIndex(_regionEntryTriggersArray, x => x.Contains(other.gameObject));

            if ((int)_currentRegion == index)
            {
                return;
            }

            _currentRegion = (RegionChange.Regions)index;
            SignalShuttle.Emit(new RegionChange(_currentRegion));
        }
        
        //TODO: uncomment to use OnGameStart instead of OnEnable
        
        // void OnGameStart(OnGameStart signal)
        // {
        //     _currentRegion = RegionChange.Regions.Hotel;
        //     SignalShuttle.Emit(new RegionChange(_currentRegion));
        // }

        // void RegisterSignals()
        // {
        //     SignalShuttle.Register<OnGameStart>(OnGameStart);
        // }
        
        // void DeregisterSignals()
        // {
        //     SignalShuttle.Deregister<OnGameStart>(OnGameStart);
        // }

        void MakeArray()
        {
            //TODO: this is terribly organized
            _regionEntryTriggersArray = new List<GameObject>[3];
            _regionEntryTriggersArray[0] = _hotelEntryTriggers;
            _regionEntryTriggersArray[1] = _beachEntryTriggers;
            _regionEntryTriggersArray[2] = _gymEntryTriggers;
        }
    }
}