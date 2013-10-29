namespace BrainShare.Documents
{
    public class ExchangeEntry
    {
        public ExchangeEntry(User user, Book book)
        {
            User = new UserData(user);
            BookSnapshot = book;
        }

        public ExchangeEntry()
        {
            
        }

        public UserData User { get; set; }
        public Book BookSnapshot { get; set; }
    }
}