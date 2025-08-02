using System;
using UnityEngine;
using LostResort.Cars;

namespace LostResort.ParticleSystems
{
    public class SandParticles : MonoBehaviour
    { 

        /// <summary>
        /// x is particle emission  at car speed 0, y is particle emission at car speed 1
        /// </summary>
        [SerializeField] private Vector2 _emissionRange;
        
        /// <summary>
        /// x is particle speed  at car speed 0, y is particle speed at car speed 1
        /// </summary>
        [SerializeField] private Vector2 _speedRange;
        
        /// <summary>
        /// 0 is front left, 1 is front right, 2 is back right, 3 is back left
        /// </summary>
        [SerializeField] private ParticleSystem[] _sandParticles;
        
        /// <summary>
        /// 0 is front left, 1 is front right, 2 is back right, 3 is back left
        /// </summary>
        [SerializeField] private Wheel[] _wheels;

        [SerializeField] private Car _car;

        private bool _isTouchingSand = false;

        [SerializeField] private Collider[] _sandColliders;

        private void Update()
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                var mainModule = _sandParticles[i].main;
                var emissionModule = _sandParticles[i].emission;
                if (Array.Exists(_sandColliders, x => x == _wheels[i].GetRaycastHit().collider))
                {
                    if (_car.GetMaxSpeedPercentage() >= 0.02f) //TODO: get rid of magic number
                    {
                        emissionModule.rateOverTimeMultiplier = Mathf.Lerp(_emissionRange.x, _emissionRange.y, _car.GetMaxSpeedPercentage());
                    }
                    else
                    {
                        emissionModule.rateOverTimeMultiplier = 0f;
                    }
                    
                    mainModule.startSpeedMultiplier = Mathf.Lerp(_speedRange.x, _speedRange.y, _car.GetMaxSpeedPercentage());
                }
                else
                {
                    emissionModule.rateOverTimeMultiplier = 0f;
                    mainModule.startSpeedMultiplier = 0f;
                }
            }
        }
    }
}