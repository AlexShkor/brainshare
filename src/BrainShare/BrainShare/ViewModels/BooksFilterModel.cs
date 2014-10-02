using System.Collections.Generic;
using System.Web.Mvc;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Infrastructure.Filters;
using BrainShare.ViewModels.Base;
using Brainshare.Infrastructure.Services.Validation;

namespace BrainShare.ViewModels
{
    public class BooksFilterModel: BaseFilterModel<BooksFilter>
    {
        public string Search { get; set; }

        public string Language { get; set; }

        public string Location { get; set; }

        public string UserName { get; set; }

        public string Author { get; set; }

        public string Category { get; set; }

        public string ISBN { get; set; }

        public bool Advanced { get; set; }

        public bool AreWhichBooksSearch { get; set; }

        public bool ExchangeOnBook { get; set; }
        public bool ExchangeOnMoney { get; set; }
        public bool ExchangeOnGift { get; set; }

        public IEnumerable<BoolItem> SearchSources { get; set; } 

        public IEnumerable<LanguageInfo> Languages { get; set; }

        public IEnumerable<CategoryData> Categories { get; set; }

        public BooksFilterModel()
        {
            ItemsPerPage = 40;
        }

        public override BooksFilter ToFilter()
        {
            var filter = base.ToFilter();
            filter.Title = Search;
            filter.Location = Location;
            filter.UserName = UserName;
            filter.Language = Language;
            filter.Author = Author;
            filter.ISBN = ISBN;
            filter.CategoryId = Category;
            filter.ExchangeTypes = new List<ExchangeOnType>();
            if (ExchangeOnBook)
            {
                filter.ExchangeTypes.Add(ExchangeOnType.Book);
            }
            if (ExchangeOnGift)
            {
                filter.ExchangeTypes.Add(ExchangeOnType.Gift);
            }
            if (ExchangeOnMoney)
            {
                filter.ExchangeTypes.Add(ExchangeOnType.Money);
            }
            return filter;
        }
    }

    public class BoolItem
    {
        public bool Value { get; set; }
        public string Text { get; set; }

        public BoolItem(bool value, string text)
        {
            Value = value;
            Text = text;
        }

        public BoolItem()
        {
            
        }
    }
}