using UnityEngine;

namespace LostResort.Levels
{
    public class StreetLamp : MonoBehaviour
    {
        [SerializeField] private GameObject glow;

        private void OnJointBreak(float breakForce)
        {
            glow.SetActive(false);
        }
    }
}
