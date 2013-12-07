using System.Collections.Generic;
using BrainShare.Infostructure.Filters;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class UsersFilterModel : BaseFilterModel<UsersFilter>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }

        public override UsersFilter ToFilter()
        {
            var filter = base.ToFilter();
            filter.FirstName = FirstName;
            filter.LastName = LastName;

            return filter;
        }
    }
}