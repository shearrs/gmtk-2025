using System;
using UnityEngine;

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

    private SignalBinding<OnLapCompleted> _testSignalBinding;
    
    void Start()
    {
        mapCompletion = (float)_lapsCompleted / _lapsToComplete;
        //SignalShuttle.AddSignal(ref _onLapCompleted);
        
        SubscribeSignals();
    }

    void Update()
    {
        // SignalShuttle<TestSignal>.Raise(new TestSignal());
    }

    private void OnDestroy()
    {
        UnsubscribeSignals();
    }

    void SubscribeSignals()
    {
        _testSignalBinding = new SignalBinding<OnLapCompleted>(OnLapCompleted);
        SignalShuttle<OnLapCompleted>.Register(_testSignalBinding);
    }

    void UnsubscribeSignals()
    {
        SignalShuttle<OnLapCompleted>.Deregister(_testSignalBinding);
    }

    void OnLapCompleted()
    {
        Debug.Log("lap completed");
    }
}
