namespace LostResort.SignalShuttles
{
    public interface ISignal { }

    public struct OnLapCompleted : ISignal { }

    public struct OnGameStart : ISignal { }

    public struct OnGameEnd : ISignal { }
}