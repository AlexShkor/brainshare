using BrainShare.Mongo;

namespace BrainShare.Services
{
    public class BooksFilter : BaseFilter
    {
        public string Title { get; set; }

        public string Location { get; set; }
    }
}