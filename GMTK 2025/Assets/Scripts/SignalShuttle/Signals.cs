//based on https://github.com/adammyhre/Unity-Event-Bus/tree/master
public interface ISignal { }

/// <summary>
/// Put signals below vvv
/// </summary>

public struct OnLapCompleted : ISignal { }

public struct TestSignal : ISignal {
    public int health;
    public int mana;
}