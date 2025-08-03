using System;
using LostResort.SignalShuttles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostResort.Score
{
    public struct AddScoreSignal : ISignal {
        public int additionalScore;

        public AddScoreSignal(int _additionalScore)
        {
            additionalScore =  _additionalScore;
        }
    }
    
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text scoreText;

        private int score;

        public int Score => score;

        private void OnEnable()
        {
            SignalShuttle.Register<AddScoreSignal>(OnAddScoreSignal);
        }

        private void OnDisable()
        {
            SignalShuttle.Deregister<AddScoreSignal>(OnAddScoreSignal);

        }
        
        private void OnAddScoreSignal(AddScoreSignal signal)
        {
            score += signal.additionalScore;
            UpdateScoreText();
        }

        public void AddScore(int additionalScore)
        {
            score += additionalScore;
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            scoreText.text = "Score: " + score;
        }
    }
}