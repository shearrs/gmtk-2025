using UnityEngine;
using LostResort.SignalShuttles;

public class TestRegionListener : MonoBehaviour
{
    private void OnEnable()
    {
        RegisterSignals();
    }

    private void OnDisable()
    {
        DeregisterSignals();
    }

    void OnRegionChange(RegionChange signal)
    {
            
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
