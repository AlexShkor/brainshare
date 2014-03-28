using Brainshare.Infrastructure.Documents;

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
        public RequestAcceptedModel(Book book, User fromUser)
        {
            Title = "Вам подарена книга";
            Message = string.Format("Книга {0} подарена вам пользователем {1}.",
                                    book.Title, fromUser.FullName);
        }

    }
}