using System;
using UnityEngine;

namespace Shears.Input
{
    public class ManagedInputProvider : MonoBehaviour, IManagedInputProvider
    {
        [SerializeField] private ManagedInputMap inputMap;

        public void DeregisterAllInputs(ManagedInputEvent action) => inputMap.DeregisterAllInputs(action);

        public void DisableAllInputs() => inputMap.DisableAllInputs();

        public void EnableAllInputs() => inputMap.EnableAllInputs();

        public IManagedInput GetInput(string name) => inputMap.GetInput(name);

        public ManagedInputGroup GetInputGroup(params (string name, ManagedInputPhase phase, ManagedInputEvent action)[] bindings) => inputMap.GetInputGroup(bindings);

        public void GetInputs(params (string name, Action<IManagedInput> action)[] inputs) => inputMap.GetInputs(inputs);
    }
}
