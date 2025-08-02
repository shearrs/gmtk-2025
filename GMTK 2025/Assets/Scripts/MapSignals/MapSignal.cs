using System;
using UnityEngine;
using LostResort.SignalShuttles;

public struct RegionChange : ISignal
{
    public enum Regions
    {
        Hotel, ConferenceCenter, Beach, Gym 
    }
}

namespace LostResort.MapSignals
{
    public class MapSignal : MonoBehaviour
    {
        //[SerializeField] private boxc
    }
}