using System;
using UnityEngine;
using LostResort.Cars;

namespace LostResort.ParticleSystems
{
    public class SandParticles : MonoBehaviour
    { 
        [SerializeField] private ParticleSystem[] _sandParticles;

        /// <summary>
        /// x is particle emission  at car speed 0, y is particle emission at car speed 1
        /// </summary>
        [SerializeField] private Vector2 _emissionRange;
        
        /// <summary>
        /// x is particle speed  at car speed 0, y is particle speed at car speed 1
        /// </summary>
        [SerializeField] private Vector2 _speedRange;

        [SerializeField] private Car _car;

        private float _carSpeed = 0f;

        private bool _isTouchingSand = false;

        [SerializeField] private Collider _sandCollider;

        private void Update()
        {
            if (_car.GetMaxSpeedPercentage() >= 0.02f) //TODO: get rid of magic number
            {
                for (int i = 0; i < _sandParticles.Length; i++)
                {
                    var emissionModule = _sandParticles[i].emission;
                    emissionModule.rateOverTimeMultiplier = Mathf.Lerp(_emissionRange.x, _emissionRange.y, _car.GetMaxSpeedPercentage());
                }
            }
            else
            {
                for (int i = 0; i < _sandParticles.Length; i++)
                {
                    var emissionModule = _sandParticles[i].emission;
                    emissionModule.rateOverTimeMultiplier = 0f;
                }
            }
            
            for (int i = 0; i < _sandParticles.Length; i++)
            {
                var mainModule = _sandParticles[i].main;
                mainModule.startSpeedMultiplier = Mathf.Lerp(_speedRange.x, _speedRange.y, _car.GetMaxSpeedPercentage());
            }
            
            // for (int i = 0; i < _sandParticles.Length; i++)
            // {
            //     var emissionModule = _sandParticles[i].emission;
            //     emissionModule.rateOverTimeMultiplier = Mathf.Lerp(_emissionRange.x, _emissionRange.y, _car.GetMaxSpeedPercentage());
            // }
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("penis");
            if (other.collider != _sandCollider)
            {
                return;
            }

            if (_isTouchingSand)
            {
                return;
            }

            _isTouchingSand = true;
            Debug.Log("penis");
        }
        
        private void OnCollisionExit(Collision other)
        {
            if (other.collider != _sandCollider)
            {
                return;
            }

            if (!_isTouchingSand)
            {
                return;
            }

            _isTouchingSand = false;
            Debug.Log("sinep");
        }
    }
}