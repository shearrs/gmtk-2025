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

        private void UpdatePassengersCarriedUI(PassengersChangedSignal signal)
        {
            int conferenceCount = 0;
            int hotelCount = 0;
            int beachCount = 0;
            int gymCount = 0;

            Debug.Log(signal.Passengers == null);

            foreach (var passenger in signal.Passengers)
            {
                if (passenger == null || passenger.TargetLocation == null)
                    continue;

                switch (passenger.TargetLocation.resortLocationName)
                {
                    case ResortLocation.ResortLocationName.Conference:
                        conferenceCount++;
                        break;
                    case ResortLocation.ResortLocationName.Hotel:
                        hotelCount++;
                        break;
                    case ResortLocation.ResortLocationName.Beach:
                        beachCount++;
                        break;
                    case ResortLocation.ResortLocationName.Gym:
                        gymCount++;
                        break;
                }
            }

            UpdateText(passengersCarriedConferenceText, conferenceCount);
            UpdateText(passengersCarriedHotelText, hotelCount);
            UpdateText(passengersCarriedBeachText, beachCount);
            UpdateText(passengersCarriedGymText, gymCount);
        }
        

        private void UpdateText(TMP_Text text, int count)
        {
            text.gameObject.SetActive(count != 0);

            text.text = "x" + count;
        }

        void OnEnable()
        {
            SignalShuttle.Register<PassengersChangedSignal>(UpdatePassengersCarriedUI);
        }

        void OnDisable()
        {
            SignalShuttle.Deregister<PassengersChangedSignal>(UpdatePassengersCarriedUI);
        }
    }

    public readonly struct PassengersChangedSignal : ISignal
    {
        private readonly List<Passenger> passengers;

        public readonly IReadOnlyCollection<Passenger> Passengers => passengers;

        public PassengersChangedSignal(List<Passenger> passengers)
        {
            this.passengers = passengers;
        }
    }

}

