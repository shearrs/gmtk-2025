using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using LostResort.SignalShuttles;

namespace LostResort.ParticleSystems
{
    public class LandingRubbleParticles : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _rubble;

        /// <summary>
        /// regions are in this order: hotel, conference center, beach, gym
        /// </summary>
        [SerializeField] private ColorRange[] _regionColors;
        
        [System.Serializable]
        private struct ColorRange
        {
            public Color minGradient;
            public Color maxGradient;
        }
        
        //add event listeners

        void OnRegionChange()
        {
            var mainModule = _rubble.main;
            mainModule.startColor = Color.Lerp(_regionColors[0].minGradient, _regionColors[0].maxGradient, Random.Range(0f, 1f));
        }
    }
}