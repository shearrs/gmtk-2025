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
    
    public class Score : MonoBehaviour
    {
        private int score;
        [SerializeField]
        private TMP_Text scoreText;

        private void OnEnable()
        {
            SignalShuttle.Register<AddScoreSignal>(OnAddScoreSignal);
        }

        private void OnDisable()
        {
            SignalShuttle.Deregister<AddScoreSignal>(OnAddScoreSignal);

        }

        private void ResetScore()
        {
            score = 0;
            UpdateScoreText();
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