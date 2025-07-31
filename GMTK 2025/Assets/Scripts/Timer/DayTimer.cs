using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostResort.SignalShuttles;

namespace LostResort.Timers
{
    public class DayTimer : MonoBehaviour
    {
        /// <summary>
        /// How long is the game (in seconds).
        /// </summary>
        [SerializeField] private float _gameDuration = 60f;
    
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
            //Debug.Log("Start timer.");
            //tracks when a second ticks up
            int lastSecond = 0;
            while (TimeElapsed<=maxTime)
            {
                TimeElapsed += Time.deltaTime;
                TimeElapsedInt = (int)Mathf.Floor(TimeElapsed);
                
                if (TimeElapsedInt > lastSecond)
                {
                    //a second has ticked
                    lastSecond = TimeElapsedInt;
                    //Debug.Log("Game Time: " + TimeElapsedInt);
                }
                if (TimeElapsedInt < lastSecond)
                {
                    //TimeElapsedInt should never be less than lastSecond
                    lastSecond = TimeElapsedInt;
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

}