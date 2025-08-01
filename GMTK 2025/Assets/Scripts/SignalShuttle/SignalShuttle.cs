using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostResort.SignalShuttles
{ 
    public static class SignalShuttle
    {
        private static readonly Dictionary<Type, ISignalBindings> signals = new();

        private interface ISignalBindings
        {
            void AddListener(Action<ISignal> listener);
            void RemoveListener(Action<ISignal> listener);
            void Invoke(ISignal signal);
        }

        private readonly struct SignalBindings<TSignal> : ISignalBindings where TSignal : ISignal
        {
            public readonly List<Action<TSignal>> listeners;

            public SignalBindings(List<Action<TSignal>> listeners)
            {
                this.listeners = listeners;
            }
            
            public readonly void AddListener(Action<TSignal> listener)
            {
                listeners.Add(listener);
            }

            public readonly void RemoveListener(Action<TSignal> listener)
            {
                listeners.Remove(listener);
            }

            public readonly void Invoke(ISignal signal)
            {
                foreach (var listener in listeners)
                    listener?.Invoke((TSignal)signal);
            }

            void ISignalBindings.AddListener(Action<ISignal> listener)
            {
                if (listener is Action<TSignal> action)
                    AddListener(action);
            }

            void ISignalBindings.RemoveListener(Action<ISignal> listener)
            {
                if (listener is Action<TSignal> action)
                    RemoveListener(action);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeSignals()
        {
            signals.Clear();
        }

        public static void Register<TSignal>(Action<TSignal> listener) where TSignal : ISignal
        {
            if (!signals.TryGetValue(typeof(TSignal), out var bindings))
            {
                bindings = new SignalBindings<TSignal>(new List<Action<TSignal>>());
                signals[typeof(TSignal)] = bindings;
            }

            if (bindings is not SignalBindings<TSignal> typedBindings)
            {
                Debug.LogError($"Signal type {typeof(TSignal)} is already registered with a different listener type.");
                return;
            }

            typedBindings.AddListener(listener);
        }

        public static void Deregister<TSignal>(Action<TSignal> listener) where TSignal : ISignal
        {
            if (signals.TryGetValue(typeof(TSignal), out var bindings))
            {
                if (bindings is not SignalBindings<TSignal> typedBindings)
                {
                    Debug.LogError($"Signal type {typeof(TSignal)} is already registered with a different listener type.");
                    return;
                }

                typedBindings.RemoveListener(listener);
            }
        }

        public static void Emit<TSignal>(TSignal signal) where TSignal : ISignal
        {
            if (signals.TryGetValue(typeof(TSignal), out var bindings))
            {
                bindings.Invoke(signal);
            }
        }
    }
}