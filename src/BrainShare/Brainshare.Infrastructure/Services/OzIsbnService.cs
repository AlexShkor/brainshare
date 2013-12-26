namespace Brainshare.Infrastructure.Services
{
    public class OzIsbnService
    {
        private readonly BooksService _booksService;
        private readonly Settings.Settings _settings;

        public OzIsbnService(BooksService books, Settings.Settings settings)
        {
            _booksService = books;
        }

        public void UpdateIsbn(string ozBookId)
        {

        }
    }
}
