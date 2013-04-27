using System.Collections.Generic;

namespace BrainShare.Documents
{
    public class Book
    {
        public string ISBN { get; set; }

        public string Title { get; set; }

        public List<string> Owners { get; set; }
    }
}