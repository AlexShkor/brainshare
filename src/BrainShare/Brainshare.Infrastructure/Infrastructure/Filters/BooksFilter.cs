using System.Collections.Generic;
using BrainShare.Infrastructure.Mongo;

namespace BrainShare.Infrastructure.Infrastructure.Filters
{
    public class BooksFilter : BaseFilter
    {
        public string ISBN { get; set; }
        public string Title { get; set; }

        public string Location { get; set; }
        public string UserName { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
        public string CategoryId { get; set; }
        public List<ExchangeOnType> ExchangeTypes { get; set; }
    }

    public enum ExchangeOnType
    {
        Book,
        Money,
        Gift
    }
}