using BrainShare.Infrastructure.Infrastructure.Filters;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class UsersFilterModel : BaseFilterModel<UsersFilter>
    {
        public UsersFilterModel()
        {
            ItemsPerPage = 48;
        }

        public string Search { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public bool Advanced { get; set; }

     

        public override UsersFilter ToFilter()
        {
            var filter = base.ToFilter();
            filter.FirstName = filter.FirstName ?? Search;
            filter.LastName = LastName;
            filter.Country = Country;

            return filter;
        }
    }
}