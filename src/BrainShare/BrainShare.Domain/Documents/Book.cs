﻿using System;
using System.Collections.Generic;
using BrainShare.Documents;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Domain.Documents
{
    public class Book
    {
        [BsonId]
        public string Id { get; set; }

        public string GoogleBookId { get; set; }
        public string OzBookId { get; set; }
        public List<string> ISBN { get; set; }
        public string Title { get; set; }
        public string SearchInfo { get; set; }
        public string Language { get; set; }
        public int PageCount { get; set; }
        public int? PublishedYear { get; set; }
        public int? PublishedMonth { get; set; }
        public int? PublishedDay { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string Country { get; set; }
        public bool IsUserReadMe { get; set; }
        public string UserComment { get; set; }

        public DateTime Added { get; set; }
        public CategoryData Category { get; set; }
        public List<string> Authors { get; set; }
        public UserData UserData { get; set; }
        public bool FromOzBy { get; set; }

        public bool Buy { get; set; }
        public bool Change { get; set; }
        public bool Gift { get; set; }

        public Book()
        {
            ISBN = new List<string>();
            Authors = new List<string>();
            UserData = new UserData();
            Added = DateTime.Now;
            Change = true;
        }

        public DateTime PublishedDate
        {
            get { return new DateTime(PublishedYear ?? 1, PublishedMonth ?? 1, PublishedDay ?? 1); }
        }
    }


    public class CategoryData
    {
        public string Id { get; set ; }
        public string Name { get; set; }

        public CategoryData(Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        public CategoryData()
        {
            
        }
    }
}