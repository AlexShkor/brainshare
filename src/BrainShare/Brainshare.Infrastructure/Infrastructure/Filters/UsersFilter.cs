using BrainShare.Infrastructure.Mongo;

namespace BrainShare.Infrastructure.Infrastructure.Filters
{
    public class UsersFilter : BaseFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
    }
}