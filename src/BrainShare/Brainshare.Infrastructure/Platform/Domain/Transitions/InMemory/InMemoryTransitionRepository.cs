using System;
using System.Collections.Generic;
using System.Linq;
using Brainshare.Infrastructure.Platform.Domain.Transitions.Interfaces;

namespace Brainshare.Infrastructure.Platform.Domain.Transitions.InMemory
{
    public class InMemoryTransitionRepository : ITransitionRepository
    {
        private readonly List<Transition> _transitions = new List<Transition>();

        public void AppendTransition(Transition transition)
        {
            _transitions.Add(transition);
        }

        public void AppendTransitions(IEnumerable<Transition> transitions)
        {
            _transitions.AddRange(transitions);
        }

        public List<Transition> GetTransitions(string streamId, int fromVersion, int toVersion)
        {
            return _transitions.Where(t =>
                t.Id.StreamId == streamId &&
                t.Id.Version >= fromVersion &&
                t.Id.Version <= toVersion)
                .ToList();
        }

        public IEnumerable<Transition> GetTransitions(int startIndex, int count)
        {
            return _transitions.Skip(startIndex).Take(count);
        }

        public Int64 CountTransitions()
        {
            return _transitions.Count;
        }

        /// <summary>
        /// Get all transitions ordered ascendantly by Timestamp of transiton
        /// Should be used only for testing and for very simple event replying 
        /// </summary>
        public IEnumerable<Transition> GetTransitions()
        {
            return _transitions;
        }

        public void RemoveTransition(string streamId, int version)
        {
            _transitions.RemoveAll(t => t.Id.StreamId == streamId && t.Id.Version == version);
        }

        public void RemoveStream(string streamId)
        {
            _transitions.RemoveAll(t => t.Id.StreamId == streamId);
        }

        public void EnsureIndexes()
        {
            // Nothing to do here. In Memory Repository does not need indexes.
        }
    }
}