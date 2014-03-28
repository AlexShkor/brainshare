using Brainshare.Infrastructure.Enums;

namespace Brainshare.Infrastructure.Documents.Data
{
    public class ExchangeEntry
    {
        public ExchangeEntry(User user, Book book, ExchangeActionEnum exchangeActionEnum = ExchangeActionEnum.Exchange)
        {
            User = new UserData(user);
            BookSnapshot = book;
            Action = exchangeActionEnum;
        }

        public ExchangeEntry(User user)
        {
            User = new UserData(user);
            Action = ExchangeActionEnum.Take;
        }

        public ExchangeEntry()
        {
            
        }

        public UserData User { get; set; }
        public Book BookSnapshot { get; set; }
        public ExchangeActionEnum Action { get; set; }
    }
}