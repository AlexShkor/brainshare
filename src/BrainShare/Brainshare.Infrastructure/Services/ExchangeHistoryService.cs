﻿using System;
using System.Collections.Generic;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Services
{
    public class ExchangeHistoryService: DocumentsService<ExchangeHistory>
    {
        public ExchangeHistoryService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<ExchangeHistory> Items
        {
            get { return Database.ExchangeHistory; }
        }

        public void SaveExchange(string initiatorId, ExchangeEntry firstUser, ExchangeEntry secondUser)
        {
            Save(new ExchangeHistory
                     {
                         Id = GenerateId(),
                         Entries = new List<ExchangeEntry>{firstUser,secondUser},
                         Initiator = initiatorId,
                         Date = DateTime.Now
                     });
        }

        public IEnumerable<ExchangeHistory> GetFor(string userId)
        {
            return
                Items.Find(
                    Query.Or(
                        Query<ExchangeHistory>.ElemMatch(x => x.Entries,
                                                         builder => builder.EQ(entry => entry.User.UserId, userId)),
                        Query<ExchangeHistory>.EQ(x => x.Initiator, userId))).SetSortOrder(SortBy<ExchangeHistory>.Descending(x=> x.Date));
        }
    }
}