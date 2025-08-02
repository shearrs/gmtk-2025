using UnityEngine;

namespace LostResort.Cars
{
    public class WheelTreadTrail : MonoBehaviour
    {
        [SerializeField] private Wheel wheel;
        [SerializeField] private TrailRenderer trailRenderer;

        private bool isLocked = false;

        private void Update()
        {
            Vector3 rotation = transform.eulerAngles;
            rotation.x = 90.0f;
            transform.eulerAngles = rotation;

            if (!isLocked)
                UpdateOnGround();
        }

        public void Enable()
        {
            trailRenderer.emitting = true;
        }

        public void Disable()
        {
            trailRenderer.emitting = false;
        }

        public void Lock()
        {
            isLocked = true;
        }

        public void Unlock()
        {
            isLocked = false;
        }

        private void UpdateOnGround()
        {
            if (!wheel.IsOnGround() && trailRenderer.emitting)
                Disable();
            else if (wheel.IsOnGround() && !trailRenderer.emitting)
                Enable();
        }
    }
}
