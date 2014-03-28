﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Brainshare.Infrastructure.Platform.Dispatching;
using Brainshare.Infrastructure.Platform.Domain.Interfaces;
using Brainshare.Infrastructure.Platform.Domain.Transitions;

namespace Brainshare.Infrastructure.Platform.Domain
{
    public class StateSpooler
    {
        private static readonly ConcurrentDictionary<MethodDescriptor, MethodInfo> _methodCache = new ConcurrentDictionary<MethodDescriptor, MethodInfo>();

        public static void Spool(AggregateState state, IEvent evnt)
        {
            if (state == null) throw new ArgumentNullException("state");
            InvokeMethodOn(state, evnt);
        }

        public static void Spool(AggregateState state, IEnumerable<IEvent> events)
        {
            if (state == null) throw new ArgumentNullException("state");

            foreach (var evnt in events)
                InvokeMethodOn(state, evnt);
        }

        private static void InvokeMethodOn(AggregateState state, IEvent evnt)
        {
            state.Invoke(evnt);
        }

        public static void Spool(AggregateState state, IEnumerable<Transition> transitions)
        {
            Spool(state, transitions.SelectMany(t => t.Events).Select(e => (IEvent) e.Data));
        }      
    }

    public class OldStateSpooler
    {
        private static readonly ConcurrentDictionary<MethodDescriptor, MethodInfo> _methodCache = new ConcurrentDictionary<MethodDescriptor, MethodInfo>();

        public static void Spool(Object state, IEvent evnt)
        {
            if (state == null) throw new ArgumentNullException("state");
            InvokeMethodOn(state, evnt);
        }

        public static void Spool(Object state, IEnumerable<IEvent> events)
        {
            if (state == null) throw new ArgumentNullException("state");

            foreach (var evnt in events)
                InvokeMethodOn(state, evnt);
        }

        public static void Spool(Object state, IEnumerable<Transition> transitions)
        {
            Spool(state, transitions.SelectMany(t => t.Events).Select(e => (IEvent)e.Data));
        }

        private static void InvokeMethodOn(Object state, Object message)
        {
            var methodDescriptor = new MethodDescriptor(state.GetType(), message.GetType());
            MethodInfo methodInfo = null;
            if (!_methodCache.TryGetValue(methodDescriptor, out methodInfo))
                _methodCache[methodDescriptor] = methodInfo = state.GetType()
                    .GetMethod("On", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                        new[] { message.GetType() }, null);

            if (methodInfo == null)
                return;

            methodInfo.Invoke(state, new[] { message });
        }
    }
}