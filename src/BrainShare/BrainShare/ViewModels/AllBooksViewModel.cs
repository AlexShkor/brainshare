using System.Collections.Generic;
using System.Linq;
using BrainShare.Domain.Documents;

namespace BrainShare.ViewModels
{
    public class AllBooksViewModel
    {
        public AllBooksViewModel(IEnumerable<Book> books)
        {
            Items = books.Select(x => new BookViewModel(x)).ToList();
        }
        public List<BookViewModel> Items { get; set; }
    }
}