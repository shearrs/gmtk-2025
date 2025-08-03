using LostResort.SignalShuttles;
using UnityEngine;

namespace LostResort.Music
{
    public readonly struct MusicChangeSignal : ISignal
    {
        public enum MusicType { Hotel, Beach, Gym }

        private readonly MusicType type;

        public readonly MusicType Type => type;

        public MusicChangeSignal(MusicType type)
        {
            this.type = type;
        }
    }
}
