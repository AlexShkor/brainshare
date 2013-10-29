using BrainShare.Services;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class BooksFilterModel: BaseFilterModel<BooksFilter>
    {
        public string Search { get; set; }

        public string Language { get; set; }

        public string Location { get; set; }

        public bool Advanced { get; set; }

        public BooksFilterModel()
        {
            ItemsPerPage = 20;
        }

        public override BooksFilter ToFilter()
        {
            var filter = base.ToFilter();
            filter.Title = Search;
            return filter;
        }
    }
}