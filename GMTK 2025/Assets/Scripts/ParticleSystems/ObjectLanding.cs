using System;
using System.Collections;
using UnityEngine;
using LostResort.Cars;

namespace LostResort.ParticleSystems
{
    public class ObjectLanding : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _impactLineParticles;

        [SerializeField] private Wheel[] _wheels;

        /// <summary>
        /// How many seconds must the car be in the air before landing will cause an impact?
        /// </summary>
        [SerializeField] private float _minimumHangTime = 1f;

        private bool _isCarGrounded = false;
        private bool _canImpact = false;

        private void Update()
        {
            if (_isCarGrounded && Array.TrueForAll(_wheels, x => !x.IsOnGround()))
            {
                _isCarGrounded = false;
                
                StopAllCoroutines();
                Debug.Log("we are in air");

                StartCoroutine(AirTime());
            }
            
            if (!_isCarGrounded && Array.Exists(_wheels, x => x.IsOnGround()))
            {
                _isCarGrounded = true;

                Debug.Log("we are hitting the ground");

                
                if (_canImpact)
                {
                    Debug.Log("Playing impact particles");
                    _impactLineParticles.Play();
                }
                else
                {
                    StopAllCoroutines();
                }
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator AirTime()
        {
            yield return new WaitForSeconds(_minimumHangTime);

            _canImpact = true;
            yield break;
        }
    }
}