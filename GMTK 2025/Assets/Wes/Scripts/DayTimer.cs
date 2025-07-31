using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostResort.SignalShuttles;

public class DayTimer : MonoBehaviour
{
    /// <summary>
    /// How long is the game (in seconds).
    /// </summary>
    [SerializeField] private float _gameDuration;

    /// <summary>
    /// How much time has elapsed in the game in float.
    /// </summary>
    public float TimeElapsed { get; private set; }
    
    /// <summary>
    /// How much time has elapsed in the game in int.
    /// </summary>
    public int TimeElapsedInt { get; private set; }

    private IEnumerator _gameTimer;
    
    void Awake()
    {
        AssignComponents();
        RegisterSignals();
    }

    private void OnDestroy()
    {
        DeregisterSignals();
        StopAllCoroutines();
    }
    
    private IEnumerator GameTimer(float maxTime)
    {
        while (TimeElapsed<=maxTime)
        {
            TimeElapsed += Time.deltaTime;

            //update TimeElapsedInt
            float tolerance = 0.001f;
            if (TimeElapsed - Mathf.Floor(TimeElapsed) <= tolerance)
            {
                TimeElapsedInt = (int)Mathf.Floor(TimeElapsed);
                //Debug.Log("Game Time: " + TimeElapsedInt);
            }
            
            yield return null;
        }
        //end the game
        SignalShuttle<OnGameEnd>.Emit(new OnGameEnd());
        yield break;
    }
    
    void StartTimer(OnGameStart signal)
    {
        StartCoroutine(_gameTimer);
    }
    
    void RegisterSignals()
    {
        SignalShuttle<OnGameStart>.Register(StartTimer);
    }
    
    void DeregisterSignals()
    {
        SignalShuttle<OnGameStart>.Deregister(StartTimer);
    }

    void AssignComponents()
    {
        _gameTimer = GameTimer(_gameDuration);
    }
}
