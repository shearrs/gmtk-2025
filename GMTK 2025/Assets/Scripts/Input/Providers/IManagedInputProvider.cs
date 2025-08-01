using System;

namespace Shears.Input
{
    public interface IManagedInputProvider
    {
        public IManagedInput GetInput(string name);

        public void GetInputs(params (string name, Action<IManagedInput> action)[] inputs);

        public ManagedInputGroup GetInputGroup(params (string name, ManagedInputPhase phase, ManagedInputEvent action)[] bindings);

        public void EnableAllInputs();

        public void DisableAllInputs();

        public void DeregisterAllInputs(ManagedInputEvent action);
    }
}
