using System;
using System.Collections.Generic;

namespace CSharpTools.Events
{
    public static class EventAggregator
    {
        private static readonly Dictionary<Type, BaseEvent> Events = new();

        public static T Get<T>() where T : BaseEvent, new()
        {
            if (!Events.ContainsKey(typeof(T)))
                Events.Add(typeof(T), new T());

            return (T)Events[typeof(T)];
        }
    }
}