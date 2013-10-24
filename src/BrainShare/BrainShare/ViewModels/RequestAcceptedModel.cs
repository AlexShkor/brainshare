using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class RequestAcceptedModel
    {
        public string Message { get; set; }
        public string Title { get; set; }

        public RequestAcceptedModel(Book book, Book onBook, User fromUser)
        {
            Title = "Запрос на обмен принят";
            Message = string.Format("Ваз запрос на обмен книги {0} пользователя {2} на вашу книгу {1} был принят.",
                                    book.Title, fromUser.FullName, onBook.Title);
        }

    }
}