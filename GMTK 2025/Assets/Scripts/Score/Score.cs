using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LostResort.Score
{
    public class Score : MonoBehaviour
    {
        private int score;
        [SerializeField]
        private TMP_Text scoreText;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /*
        public struct AddScore : ISignal {
            public int additionalScore;
        }

        */

        private void ResetScore()
        {
            score = 0;
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