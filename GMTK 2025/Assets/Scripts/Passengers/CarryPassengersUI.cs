using System;
using System.Collections.Generic;
using LostResort.SignalShuttles;
using TMPro;
using UnityEngine;

namespace LostResort.Passengers
{
    public class CarryPassengersUI : MonoBehaviour
    {

        [SerializeField]
        private TMP_Text passengersCarriedConferenceText;
        [SerializeField]
        private TMP_Text passengersCarriedHotelText;
        [SerializeField]
        private TMP_Text passengersCarriedBeachText;
        [SerializeField]
        private TMP_Text passengersCarriedGymText;
        
        private Dictionary<ResortLocation.ResortLocationName, int> carriedPassengers = new  Dictionary<ResortLocation.ResortLocationName, int>();

        private void UpdatePassengersCarriedUI(PassengersChangedSignal signal)
        {

            carriedPassengers.TryAdd(signal.resortLocation, 0);
            
            ResortLocation.ResortLocationName resortLocation = signal.resortLocation;
            
            if(signal.passengerEntered)
                carriedPassengers[resortLocation]++;
            else
            {
                carriedPassengers[resortLocation]--;

            }

            TMP_Text passengersCarriedText;
            
            
            
            switch (resortLocation)
            {
                case ResortLocation.ResortLocationName.Conference:
                    passengersCarriedText = passengersCarriedConferenceText;
                    break;
                case ResortLocation.ResortLocationName.Hotel:
                    passengersCarriedText = passengersCarriedHotelText;
                    break;
                case ResortLocation.ResortLocationName.Beach:
                    passengersCarriedText = passengersCarriedBeachText;
                    break;
                case ResortLocation.ResortLocationName.Gym:
                    passengersCarriedText = passengersCarriedGymText;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resortLocation), resortLocation, null);
            }
            
            
            UpdatePassengersCarriedText(passengersCarriedText, resortLocation);
        }
        

        private void UpdatePassengersCarriedText(TMP_Text passengersCarriedText, ResortLocation.ResortLocationName resortLocation)
        {
            passengersCarriedText.gameObject.SetActive(carriedPassengers[resortLocation] != 0);

            passengersCarriedText.text = "x" + carriedPassengers[resortLocation];
        }

        void OnEnable()
        {
            RegisterSignals();
        }

        void OnDisable()
        {
            DeregisterSignals();
        }
        
        void RegisterSignals()
        {
            SignalShuttle.Register<PassengersChangedSignal>(UpdatePassengersCarriedUI);
        }
        
        void DeregisterSignals()
        {
            SignalShuttle.Deregister<PassengersChangedSignal>(UpdatePassengersCarriedUI);
        }
        
    }

    public struct PassengersChangedSignal : ISignal
    {
        public bool passengerEntered {private set; get;}
        public ResortLocation.ResortLocationName resortLocation {private set; get;}

        public PassengersChangedSignal(bool passengerEntered, ResortLocation.ResortLocationName resortLocation)
        {
            this.passengerEntered = passengerEntered;
            this.resortLocation = resortLocation;
        }
    }

}

