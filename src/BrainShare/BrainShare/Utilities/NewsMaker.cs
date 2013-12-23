namespace BrainShare.Utilities
{
    public class NewsMaker
    {
        public static string UserHaveBookMessage(string ownerName, string ownerId, string title, string bookId)
        {
            return string.Format("У пользователя {0} появилась книга {1}",ownerName,title);
        }

        public static string UserSearchedBookMessage(string searcherName, string title)
        {
            return string.Format("Пользователь {0} ищет книгу {1}", searcherName, title);
        }
    }
}