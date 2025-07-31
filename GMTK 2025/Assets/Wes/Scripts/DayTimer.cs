using System;
using UnityEngine;
using LostResort.SignalShuttles;

public class DayTimer : MonoBehaviour
{
    /// <summary>
    /// how many laps it takes to finish the day
    /// </summary>
    [SerializeField] private int _lapsToComplete;

    /// <summary>
    /// how many laps the player has finished
    /// </summary>
    private int _lapsCompleted;

    /// <summary>
    /// what percent of the map the player has completed
    /// </summary>
    private float mapCompletion;

    //private Signals.SignalNoArgs _onLapCompleted;

    //private SignalBinding<OnLapCompleted> _testSignalBinding;
    
    void Awake()
    {
        mapCompletion = (float)_lapsCompleted / _lapsToComplete;
        //SignalShuttle.AddSignal(ref _onLapCompleted);
        
        RegisterSignals();
    }

    void Update()
    {
        // SignalShuttle<TestSignal>.Raise(new TestSignal());
    }

    private void OnDestroy()
    {
        DeregisterSignals();
    }

    void RegisterSignals()
    {
        SignalShuttle<OnLapCompleted>.Register(OnLapCompleted);
    }

    void DeregisterSignals()
    {
        SignalShuttle<OnLapCompleted>.Deregister(OnLapCompleted);
    }

    void OnLapCompleted(OnLapCompleted signal)
    {
        Debug.Log("lap completed");
    }
}
