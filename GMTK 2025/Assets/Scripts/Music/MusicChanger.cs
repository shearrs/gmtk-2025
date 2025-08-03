using LostResort.SignalShuttles;
using UnityEngine;

namespace LostResort.Music
{
    public class MusicChanger : MonoBehaviour
    {
        [SerializeField] private MusicChangeSignal.MusicType type;

        private void OnTriggerEnter(Collider other)
        {
            SignalShuttle.Emit(new MusicChangeSignal(type));
        }
    }
}
