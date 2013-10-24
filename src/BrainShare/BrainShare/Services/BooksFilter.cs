using BrainShare.Mongo;

namespace BrainShare.Services
{
    public class BooksFilter : BaseFilter
    {
        public string TitleContains { get; set; }

        public string Location { get; set; }
    }
}