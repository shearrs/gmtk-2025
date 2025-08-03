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
        /// If the timer should loop
        /// </summary>
        [SerializeField] private bool doesLoop;
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
                }
                if (TimeElapsedInt < lastSecond)
                {
                    lastSecond = TimeElapsedInt;
                }
                
                yield return null;
            }

            if (doesLoop)
            {
                ResetUI();
                StopAllCoroutines();
                _gameTimer = GameTimer(_gameDuration);
                StartCoroutine(_gameTimer);
                yield break;
            }
            //end the game
            SignalShuttle.Emit(new OnGameEnd());
            yield break;
        }
        
        void ResetUI()
        {
            if (!_timerCircle.enabled)
            {
                _timerCircle.enabled = true;
            }

            foreach (Image hourDot in _hourDots)
            {
                if (!hourDot.enabled)
                {
                    hourDot.enabled = true;
                }
            }
        }
        
        void StartTimer(OnGameStart signal)
        {
            StartCoroutine(_gameTimer);
        }
        
        void RegisterSignals()
        {
            SignalShuttle.Register<OnGameStart>(StartTimer);
            SignalShuttle.Register<OnGameEnd>(ClearUI);
        }
        
        void DeregisterSignals()
        {
            SignalShuttle.Deregister<OnGameStart>(StartTimer);
            SignalShuttle.Deregister<OnGameEnd>(ClearUI);
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