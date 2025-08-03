using Shears.Tweens;
using UnityEngine;

namespace LostResort.Levels
{
    public class DoorGate : Gate
    {
        [SerializeField] private bool openByDefault = false;
        [SerializeField] private Transform firstHalf;
        [SerializeField] private Transform secondHalf;
        [SerializeField] private Vector3 firstClosed;
        [SerializeField] private Vector3 firstOpen;
        [SerializeField] private Vector3 secondClosed;
        [SerializeField] private Vector3 secondOpen;
        [SerializeField] private TweenData tweenData;

        private bool isOpen;
        private ITween firstTween;
        private ITween secondTween;

        private void Awake()
        {
            isOpen = openByDefault;
        }

        public override void ToggleOpen()
        {
            if (firstTween != null && firstTween.IsPlaying)
            {
                firstTween.Stop();
                firstTween.Dispose();
            }

            if (secondTween != null && secondTween.IsPlaying)
            {
                secondTween.Stop();
                secondTween.Dispose();
            }

            Vector3 firstPosition;
            Vector3 secondPosition;

            if (isOpen)
            {
                firstPosition = firstClosed;
                secondPosition = secondClosed;
            }
            else
            {
                firstPosition = firstOpen;
                secondPosition = secondOpen;
            }

            firstTween = firstHalf.DoMoveLocalTween(firstPosition, tweenData);
            secondTween = secondHalf.DoMoveLocalTween(secondPosition, tweenData);

            isOpen = !isOpen;
        }
    }
}
