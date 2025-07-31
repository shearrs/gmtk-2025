//based on https://github.com/adammyhre/Unity-Event-Bus/tree/master
using System.Collections.Generic;
using UnityEngine;

public static class SignalShuttle<T> where T : ISignal {
    static readonly HashSet<ISignalBinding<T>> bindings = new HashSet<ISignalBinding<T>>();
    
    public static void Register(SignalBinding<T> binding) => bindings.Add(binding);
    public static void Deregister(SignalBinding<T> binding) => bindings.Remove(binding);

    public static void Raise(T @signal) {
        var snapshot = new HashSet<ISignalBinding<T>>(bindings);

        foreach (var binding in snapshot) {
            if (bindings.Contains(binding)) {
                binding.OnSignal.Invoke(@signal);
                binding.OnSignalNoArgs.Invoke();
            }
        }
    }

    static void Clear() {
        Debug.Log($"Clearing {typeof(T).Name} bindings");
        bindings.Clear();
    }
}