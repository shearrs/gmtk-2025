using Shears.Tweens;
using UnityEngine;

namespace LostResort.Passengers
{
    public class ExclamationMark : MonoBehaviour
    {
        [SerializeField] private TweenData spawnTweenData;
        [SerializeField] private TweenData growTweenData;

        private float startingScoreWhenDroppedOff;
        
        private ExclamationMarkState exclamationMarkState = ExclamationMarkState.notYetEnabled;
        private enum ExclamationMarkState
        {
            notYetEnabled,
            normal,
            growingAndShrinking
        }
        
        private void EnableExclamationMark()
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
        
        public void DisableExclamationMark()
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
        
        public void InitializeStartingScoreWhenDroppedOff(float _startingScoreWhenDroppedOff)
        {
            startingScoreWhenDroppedOff = _startingScoreWhenDroppedOff;
        }

        private void Spawn()
        {
            EnableExclamationMark();
            Vector3 jumpVector = transform.localPosition + Vector3.up;
            transform.DoMoveLocalTween(jumpVector, spawnTweenData);
        }

        private void BeginGrowAndShrink()
        {
            Vector3 originalScale = transform.localScale;
            Vector3 increasedScale = originalScale * 1.5f;

            transform.DoScaleLocalTween(increasedScale, growTweenData);
        }

        public void ReceiveScoreWhenDroppedOff(float scoreWhenDroppedOff)
        {
            //this is already the final state
            if (exclamationMarkState == ExclamationMarkState.growingAndShrinking)
            {
                return;
            }
            
            if (scoreWhenDroppedOff > startingScoreWhenDroppedOff / 2)
            {
                return;
            }

            //when we have dipped below half the startingScoreWhenDroppedOff
            if (exclamationMarkState == ExclamationMarkState.notYetEnabled)
            {
                exclamationMarkState =  ExclamationMarkState.normal;
                Spawn();
                return;
            }
            
            if (scoreWhenDroppedOff > startingScoreWhenDroppedOff / 4)
            {
                return;
            }

            //when we have dipped below a fourth the startingScoreWhenDroppedOff
            if (exclamationMarkState == ExclamationMarkState.normal)
            {
                exclamationMarkState =  ExclamationMarkState.growingAndShrinking;
                BeginGrowAndShrink();
                return;
            }
        }
    }
}
