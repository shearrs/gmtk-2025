using System;
using UnityEngine;
using LostResort.Cars;

namespace LostResort.ParticleSystems
{
    public class ObjectLanding : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _impactLineParticles;
        [SerializeField] private ParticleSystem _rubble;

        [SerializeField] private Wheel[] _wheels;
        [SerializeField] private Car _car;

        private bool _isCarGrounded = false;

        /// <summary>
        /// x is particle speed at car speed 0, y is particle speed at car speed 1
        /// </summary>
        [SerializeField] private Vector2 _speedRange;

        private float _carSpeed = 0f;

        private void Update()
        {
            
            
            for (int i = 0; i < _impactLineParticles.Length; i++)
            {
                var mainModule = _impactLineParticles[i].main;
                mainModule.startSpeedMultiplier = Mathf.Lerp(_speedRange.x, _speedRange.y, _carSpeed);
            }
        }
    }
}