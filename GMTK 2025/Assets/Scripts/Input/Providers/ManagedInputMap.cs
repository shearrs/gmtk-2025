using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Shears.Input
{
    [CreateAssetMenu(fileName = "New Managed Input Map", menuName = "Shears Library/Managed Input/Map")]
    public class ManagedInputMap : ScriptableObject, IManagedInputProvider
    {
        [Header("Input Actions")]
        [SerializeField] private InputActionAsset inputActions;
        [SerializeField] private string actionMapName;

        private RuntimeInputMap runtimeMap;

        internal string ID { get; private set; }
        internal InputActionAsset InputActions => inputActions;

        public ManagedInputUser User { get => runtimeMap.User; set => runtimeMap.User = value; }

        private void OnValidate()
        {
            ID ??= Guid.NewGuid().ToString();
        }

        public IManagedInput GetInput(string name) => GetRuntimeMap().GetInput(name);

        public void GetInputs(params (string name, Action<IManagedInput> action)[] inputs)
            => GetRuntimeMap().GetInputs(inputs);

        public ManagedInputGroup GetInputGroup(params (string name, ManagedInputPhase phase, ManagedInputEvent action)[] bindings)
            => GetRuntimeMap().GetInputGroup(bindings);

        public void EnableAllInputs() => GetRuntimeMap().EnableAllInputs();

        public void DisableAllInputs() => GetRuntimeMap().DisableAllInputs();

        public void DeregisterAllInputs(ManagedInputEvent action) => GetRuntimeMap().DeregisterAllInputs(action);

        private RuntimeInputMap GetRuntimeMap()
        {
            if (runtimeMap == null)
            {
                var gameObject = new GameObject("[Runtime] " + name)
                {
                    hideFlags = HideFlags.HideInHierarchy
                };
                runtimeMap = gameObject.AddComponent<RuntimeInputMap>();

                runtimeMap.Initialize(inputActions, actionMapName);
            }

            return runtimeMap;
        }
    }
}
