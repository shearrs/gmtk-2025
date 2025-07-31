using UnityEngine;
using LostResort.SignalShuttles;

public class test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SignalShuttle<OnLapCompleted>.Emit(new OnLapCompleted());
    }

    // Update is called once per frame
    void Update()
    {
        //SignalShuttle<OnLapCompleted>.Raise(new OnLapCompleted());
    }
}
