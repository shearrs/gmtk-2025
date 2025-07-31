//based on https://github.com/adammyhre/Unity-Event-Bus/tree/master

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LostResort.SignalShuttles
{
    public static class SignalShuttle<T> where T : ISignal {
        static readonly HashSet<ISignalBinding<T>> bindings = new HashSet<ISignalBinding<T>>();

        static readonly Dictionary<Action<T>, SignalBinding<T>> signalBindings = new();
    
        //public static void Register(SignalBinding<T> binding) => bindings.Add(binding);
        //public static void Deregister(SignalBinding<T> binding) => bindings.Remove(binding);

        public static void Register(Action<T> listener)
        {
            if (!signalBindings.TryGetValue(listener, out var binding))
            {
                binding = new SignalBinding<T>(listener);
                signalBindings.Add(listener, binding);
            }

            bindings.Add(signalBindings[listener]);
        }

        public static void Deregister(Action<T> listener)
        {
            if (signalBindings.TryGetValue(listener, out var binding))
            {
                //signalBindings.Remove(listener); TODO: for some reason key is not found when deregistering
                bindings.Remove(signalBindings[listener]);
            }
        }

        public static void Emit(T @signal) {
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
}