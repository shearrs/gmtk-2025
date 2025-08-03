using LostResort.Cars;
using LostResort.Music;
using LostResort.Score;
using LostResort.SignalShuttles;
using Shears.Tweens;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

namespace LostResort.Levels
{
    public class GameEndScreen : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Car car;
        [SerializeField] private MusicManager musicManager;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private Canvas[] scoreUI;

        [Header("Canvas")]
        [SerializeField] private Canvas endCanvas;
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private Button continueButton;

        private bool isEnding = false;

        private void OnEnable()
        {
            SignalShuttle.Register<GameEndSignal>(OnGameEnd);
        }

        private void OnDisable()
        {
            SignalShuttle.Deregister<GameEndSignal>(OnGameEnd);
        }

        private void OnGameEnd(GameEndSignal signal)
        {
            if (isEnding)
                return;

            StartCoroutine(IEGameEnd());
        }

        private IEnumerator IEGameEnd()
        {
            isEnding = true;

            car.Rigidbody.isKinematic = true;
            car.Disable();

            musicManager.SetVolume(0.05f);

            foreach (var canvas in scoreUI)
                canvas.enabled = false;

            endCanvas.enabled = true;
            scoreText.enabled = false;
            continueButton.enabled = false;
            background.color = Color.clear;
            var backgroundColor = Color.indianRed;
            backgroundColor.a = 0.85f;

            var tween = background.DoColorTween(backgroundColor);

            while (tween.IsPlaying)
                yield return null;

            void textUpdate(float t)
            {
                scoreText.color = Color.Lerp(Color.clear, Color.lightGoldenRodYellow, t);
            }

            scoreText.text = "SCORE: " + scoreManager.Score;
            scoreText.enabled = true;
            scoreText.color = Color.clear;
            tween = TweenManager.DoTween(textUpdate);

            while (tween.IsPlaying)
                yield return null;

            continueButton.enabled = true;
        }
    }
}
