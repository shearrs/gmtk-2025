using Shears.Tweens;
using UnityEngine;

namespace LostResort.Levels
{
    public class BridgeGate : Gate
    {
        [SerializeField] private bool openByDefault = false;
        [SerializeField] private Transform rotateTarget;
        [SerializeField] private Vector3 openRotation;
        [SerializeField] private Vector3 closeRotation;
        [SerializeField] private TweenData tweenData;

        private bool isOpen;
        private ITween tween;

        private void Awake()
        {
            if (openByDefault)
                isOpen = true;
        }

        public override void ToggleOpen()
        {
            if (tween != null && tween.IsPlaying)
            {
                tween.Stop();
                tween.Dispose();
            }

            Vector3 endRotation;

            if (isOpen)
                endRotation = closeRotation;
            else
                endRotation = openRotation;

            tween = rotateTarget.DoRotateLocalTween(Quaternion.Euler(endRotation), true, tweenData);
            isOpen = !isOpen;
        }
    }
}