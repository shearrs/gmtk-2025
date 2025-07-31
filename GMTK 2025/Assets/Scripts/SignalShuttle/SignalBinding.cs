//based on https://github.com/adammyhre/Unity-Event-Bus/tree/master
using System;

namespace LostResort.SignalShuttles
{
    public interface ISignalBinding<T> {
        public Action<T> OnSignal { get; set; }
        public Action OnSignalNoArgs { get; set; }
    }

    public class SignalBinding<T> : ISignalBinding<T> where T : ISignal {
        Action<T> onSignal = _ => { };
        Action onSignalNoArgs = () => { };

        Action<T> ISignalBinding<T>.OnSignal {
            get => onSignal;
            set => onSignal = value;
        }

        Action ISignalBinding<T>.OnSignalNoArgs {
            get => onSignalNoArgs;
            set => onSignalNoArgs = value;
        }

        public SignalBinding(Action<T> onSignal) => this.onSignal = onSignal;
        public SignalBinding(Action onSignalNoArgs) => this.onSignalNoArgs = onSignalNoArgs;
    
        public void Add(Action onSignal) => onSignalNoArgs += onSignal;
        public void Remove(Action onSignal) => onSignalNoArgs -= onSignal;
    
        public void Add(Action<T> onSignal) => this.onSignal += onSignal;
        public void Remove(Action<T> onSignal) => this.onSignal -= onSignal;
    }
}