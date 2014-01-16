using BrainShare.Documents;
using BrainShare.Domain.Enums;

namespace BrainShare.Domain.Documents.Data
{
    public class ExchangeEntry
    {
        public ExchangeEntry(User user, Book book, ExchangeEntryType exchangeEntryType = ExchangeEntryType.Exchange)
        {
            User = new UserData(user);
            BookSnapshot = book;
            ExchangeEntryType = exchangeEntryType;
        }

        public ExchangeEntry(User user)
        {
            User = new UserData(user);
            ExchangeEntryType = ExchangeEntryType.GiftReceiver;
        }

        public ExchangeEntry()
        {
            
        }

        public UserData User { get; set; }
        public Book BookSnapshot { get; set; }
        public ExchangeEntryType ExchangeEntryType { get; set; }
    }
}