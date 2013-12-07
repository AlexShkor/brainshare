using BrainShare.Mongo;

namespace BrainShare.Infostructure.Filters
{
    public class UsersFilter : BaseFilter
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}