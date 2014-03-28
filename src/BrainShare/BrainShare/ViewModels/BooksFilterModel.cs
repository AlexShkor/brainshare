using System.Collections.Generic;
using Brainshare.Infrastructure.Infrastructure.Filters;
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

        public string ISBN { get; set; }

        public bool Advanced { get; set; }

        public IEnumerable<LanguageInfo> Languages { get; set; }

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
            return filter;
        }
    }
}