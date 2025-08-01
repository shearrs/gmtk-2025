using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LostResort.SignalShuttles;
using UnityEngine.UI;

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
        /// How much time has elapsed in the game in float between 0f and 1f.
        /// </summary>
        public float TimeElapsedPercent { get; private set; }
        
        /// <summary>
        /// How much time has elapsed in the game in int.
        /// </summary>
        public int TimeElapsedInt { get; private set; }
    
        private IEnumerator _gameTimer;

        [SerializeField] private Image _timerCircle;
        [SerializeField] private Image[] _hourDots;
        
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
            TimeElapsed = 0f;
            TimeElapsedInt = 0;
            TimeElapsedPercent = 0f;
            //tracks when a second ticks up
            int lastSecond = 0;
            //how many hours have passed. number of total hours is determined by _hourDots.Length
            int hoursElapsed = 0;
            while (TimeElapsed <= maxTime)
            {
                TimeElapsed += Time.deltaTime;
                TimeElapsedInt = (int)Mathf.Floor(TimeElapsed);
                TimeElapsedPercent = Mathf.Clamp01(TimeElapsed / maxTime);
                
                //update UI
                _timerCircle.fillAmount = Mathf.Lerp(1f, 0f, TimeElapsed / maxTime);
                if (TimeElapsedPercent > ((float)hoursElapsed + 1) / _hourDots.Length)
                {
                    if (hoursElapsed < _hourDots.Length)
                    {
                        _hourDots[hoursElapsed].enabled = false;
                    }
                    hoursElapsed++;
                }
                
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
            SignalShuttle<OnGameEnd>.Register(ClearUI);
        }
        
        void DeregisterSignals()
        {
            SignalShuttle<OnGameStart>.Deregister(StartTimer);
            SignalShuttle<OnGameEnd>.Deregister(ClearUI);
        }
    
        void AssignComponents()
        {
            _gameTimer = GameTimer(_gameDuration);
        }

        void ClearUI(OnGameEnd signal)
        {
            if (_timerCircle.enabled)
            {
                _timerCircle.enabled = false;
            }

            foreach (Image hourDot in _hourDots)
            {
                if (hourDot.enabled)
                {
                    hourDot.enabled = false;
                }
            }
        }
    }

}