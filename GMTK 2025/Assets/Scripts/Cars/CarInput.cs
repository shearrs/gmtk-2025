using Shears.Input;
using UnityEngine;

namespace LostResort.Cars
{
    [DefaultExecutionOrder(-100)]
    public class CarInput : MonoBehaviour
    {
        [SerializeField] private ManagedInputMap inputMap;

        public IManagedInput MoveInput { get; private set; }
        public IManagedInput DriftInput { get; private set; }

        private void Awake()
        {
            inputMap.GetInputs(
                ("Move", (i) => MoveInput = i),
                ("Drift", (i) => DriftInput = i)
            );
        }

        public void Enable()
        {
            inputMap.EnableAllInputs();
        }

        public void Disable()
        {
            inputMap.DisableAllInputs();
        }
    }
}
