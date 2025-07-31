using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LostResort.Passengers
{
    public class PassengerData
    {


        /// <summary>
        /// Takes in a starting location. Set's the dropOffLocation to a random other location.
        /// </summary>
        /// <param name="_startingLocation">The location that the passenger started at.</param>
        public PassengerData(LocationType _startingLocation)
        {
            startingLocation = _startingLocation;
            Array locations = Enum.GetValues(typeof(LocationType));


            int randomIndex = Random.Range(0, locations.Length);

            while (randomIndex == (int)_startingLocation)
            {
                randomIndex = Random.Range(0, locations.Length);
            }

            LocationType location = (LocationType)locations.GetValue(randomIndex);

            dropOffLocation = location;

        }

        public LocationType startingLocation { get; private set; }
        public LocationType dropOffLocation { get; private set; }


    }
}
