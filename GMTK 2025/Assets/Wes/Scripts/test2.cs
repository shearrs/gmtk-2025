using System;
using UnityEngine;
using LostResort.SignalShuttles;

public class test2 : MonoBehaviour
{
    private void Awake()
    {
        RegisterSignals();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        DeregisterSignals();
    }

    void RegisterSignals()
    {
        SignalShuttle<OnGameStart>.Register(Startdf);
        SignalShuttle<OnGameEnd>.Register(Enddfdfd);
    }
    
    void DeregisterSignals()
    {
        SignalShuttle<OnGameStart>.Deregister(Startdf);
        SignalShuttle<OnGameEnd>.Deregister(Enddfdfd);
    }

    void Startdf(OnGameStart signal)
    {
        Debug.Log("This works");
    }

    void Enddfdfd(OnGameEnd signal)
    {
        Debug.Log("game end");
    }
}
