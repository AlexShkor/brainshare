namespace BrainShare.Utilities
{
    public class NewsMaker
    {
        public static string UserHaveBookMessage(string ownerName, string ownerId, string title, string bookId)
        {
            return string.Format("У пользователя {0} появилась книга {1}",ownerName,title);
        }
    }
}