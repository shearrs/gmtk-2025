using System;
using UnityEngine;

namespace LostResort.ParticleSystems
{
    public class ExhaustParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _exhaustParticles;

        /// <summary>
        /// x is particle speed at car speed 0, y is particle speed at car speed 1
        /// </summary>
        [SerializeField] private Vector2 _speedRange;

        private float _carSpeed = 0f;

        private void Update()
        {
            for (int i = 0; i < _exhaustParticles.Length; i++)
            {
                var mainModule = _exhaustParticles[i].main;
                mainModule.startSpeedMultiplier = Mathf.Lerp(_speedRange.x, _speedRange.y, _carSpeed);
            }
        }
    }
}