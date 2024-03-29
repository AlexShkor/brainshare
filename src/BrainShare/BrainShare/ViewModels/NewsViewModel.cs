﻿using System.Collections.Generic;
using System.Linq;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;

namespace BrainShare.ViewModels
{
    public class NewsViewModel
    {
        public NewsViewModel(List<UserNewsInfo> userNewsInfo,IEnumerable<News> news )
        {
            News = new List<NewsItemViewModel>();
            foreach (var newsItem in news)
            {
                var appopriateInfo = userNewsInfo.Single(i => i.Id == newsItem.Id);

                News.Add(new NewsItemViewModel
                    {
                        Id = appopriateInfo.Id,
                        Message = newsItem.Message,
                        WasRead = appopriateInfo.WasRead,
                        Title = newsItem.Title,
                    });
            }
        }

        public List<NewsItemViewModel> News { get; set; }
    }

    public class NewsItemViewModel
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public bool WasRead { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; }
    }
}