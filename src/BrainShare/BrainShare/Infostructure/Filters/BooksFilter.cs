using BrainShare.Mongo;

namespace BrainShare.Infostructure.Filters
{
    public class BooksFilter : BaseFilter
    {
        public string ISBN { get; set; }
        public string Title { get; set; }

        public string Location { get; set; }
        public string UserName { get; set; }
        public string Author { get; set; }
        public string Language { get; set; }
    }
}