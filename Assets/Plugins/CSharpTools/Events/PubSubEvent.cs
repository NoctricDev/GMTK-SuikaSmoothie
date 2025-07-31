using System;
using System.Collections.Generic;

namespace CSharpTools.Events
{
    public abstract class BaseEvent
    {
    }

    public class PubSubEvent : BaseEvent
    {
        private readonly List<Action> _actions = new();

        public void Publish()
        {
            _actions.ForEach(a => a.Invoke());
        }

        public void Subscribe(Action action)
        {
            _actions.Add(action);
        }

        public void Unsubscribe(Action action)
        {
            _actions.Remove(action);
        }
    }

    public class PubSubEvent<T> : BaseEvent
    {
        private readonly List<Action<T>> _actions = new();

        public void Subscribe(Action<T> action)
        {
            _actions.Add(action);
        }

        public void Unsubscribe(Action<T> action)
        {
            _actions.Remove(action);
        }

        public void Publish(T value)
        {
            _actions.ForEach(a => a.Invoke(value));
        }
    }
}