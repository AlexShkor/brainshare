﻿using System;
using System.Collections.Generic;
using System.Linq;
using Brainshare.Infrastructure.Platform.Domain.Transitions.Exceptions;
using Brainshare.Infrastructure.Platform.Domain.Transitions.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Platform.Domain.Transitions.Mongo
{
    public class MongoTransitionRepository : ITransitionRepository
    {
        private const string _concurrencyException = "E1100";
        private readonly MongoTransitionServer _transitionServer;
        private readonly MongoTransitionSerializer _serializer;

        public MongoTransitionRepository(String connectionString, String transitionsCollectionName = null)
        {
            _serializer = new MongoTransitionSerializer();
            _transitionServer = new MongoTransitionServer(connectionString, transitionsCollectionName);

            EnsureIndexes();
        }

        private Dictionary<BsonDocument, IndexKeysBuilder> RequiredIndexes()
        {
            return new Dictionary<BsonDocument, IndexKeysBuilder>
            {
                {new BsonDocument("_id.StreamId", 1), IndexKeys.Ascending("_id.StreamId")},
                {new BsonDocument("_id.Version", 1), IndexKeys.Ascending("_id.Version")},
                {new BsonDocument("Timestamp", 1), IndexKeys.Ascending("Timestamp")},
                {new BsonDocument
                    {
                        new BsonElement("Timestamp", 1),
                        new BsonElement("_id.Version", 1),
                         
                    }, IndexKeys.Ascending("Timestamp", "_id.Version")},
            };
        }

        public void EnsureIndexes()
        {
            var indexes = _transitionServer.Transitions.GetIndexes().Select(x => x.RawDocument["key"] as BsonDocument).ToList();
            foreach (var index in RequiredIndexes())
            {
                if (!indexes.Contains(index.Key))
                    _transitionServer.Transitions.CreateIndex(index.Value);
            }

            _transitionServer.Snapshots.EnsureIndex(IndexKeys.Ascending("_id.StreamId").Descending("_id.Version"));
            _transitionServer.Snapshots.EnsureIndex(IndexKeys.Ascending("_id.StreamId"));
        }

        public void AppendTransition(Transition transition)
        {
            // skip saving empty transition
            if (transition.Events.Count < 1)
                return;

            var doc = _serializer.Serialize(transition);

            try
            {
                _transitionServer.Transitions.Insert(doc, WriteConcern.Acknowledged);
            }
            catch (MongoException e)
            {
                if (!e.Message.Contains(_concurrencyException))
                    throw;

                throw new DuplicateTransitionException(transition.Id.StreamId, transition.Id.Version, e);
            }
        }

        public void AppendTransitions(IEnumerable<Transition> transitions)
        {
            // skip saving empty transition
            var list = transitions.Where(x => x.Events.Any()).ToList();
            if (list.Count < 1)
                return;

            var docs = list.Select(t => _serializer.Serialize(t));

            try
            {
                _transitionServer.Transitions.InsertBatch(docs, SafeMode.True);
            }
            catch (MongoException e)
            {
                if (!e.Message.Contains(_concurrencyException))
                    throw;

                throw new DuplicateTransitionException("Several Streams", 0, e);
            }
        }

        public List<Transition> GetTransitions(string streamId, int fromVersion, int toVersion)
        {
            var query = Query.And(Query.EQ("_id.StreamId", streamId),
                                  Query.GTE("_id.Version", fromVersion),
                                  Query.LTE("_id.Version", toVersion));

            var sort = SortBy.Ascending("_id.Version");

            var docs = _transitionServer.Transitions.FindAs<BsonDocument>(query)
                .SetSortOrder(sort)
                .ToList();

            // Check that such stream exists
            if (docs.Count < 1)
                return new List<Transition>();

            var transitions = docs.Select(_serializer.Deserialize).ToList();

            return transitions;
        }

        public IEnumerable<Transition> GetTransitions(int startIndex, int count)
        {
            var query = Query.And();

            var sort = SortBy.Ascending("Timestamp", "_id.Version");

            var docs = _transitionServer.Transitions.FindAs<BsonDocument>(query)
                .SetSkip(startIndex)
                .SetLimit(count)
                .SetSortOrder(sort)
                .ToList();

            var transitions = docs.Select(_serializer.Deserialize).ToList();

            return transitions;
        }

        public Int64 CountTransitions()
        {
            return _transitionServer.Transitions.Count();
        }

        /// <summary>
        /// Get all transitions ordered ascendantly by Timestamp of transiton
        /// Should be used only for testing and for very simple event replying 
        /// </summary>
        public IEnumerable<Transition> GetTransitions()
        {
            var docs = _transitionServer.Transitions.FindAllAs<BsonDocument>()
                .SetSortOrder(SortBy.Ascending("Timestamp", "_id.Version"));

            var transitions = docs.Select(_serializer.Deserialize);

            return transitions;
        }

        public void RemoveTransition(string streamId, int version)
        {
            var id = _serializer.SerializeTransitionId(new TransitionId(streamId, version));
            var query = new BsonDocument { { "_id", id } };

            _transitionServer.Transitions.Remove(new QueryDocument(query));
        }

        public void RemoveStream(String streamId)
        {
            var query = new BsonDocument { { "_id.StreamId", streamId } };
            _transitionServer.Transitions.Remove(new QueryDocument(query));
        }
    }
}
