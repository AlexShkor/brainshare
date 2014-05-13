using BrainShare.Domain.Documents;
using RestSharp.Extensions;

namespace BrainShare.Controllers
{
    public class LinkedGroupItem
    {
        public string Id { get; set; }

        public string Name { get; set; }
        
        public string ExpirationDate { get; set; }

        public bool IsAuthorized { get; set; }

        public LinkedGroupItem(LinkedGroup linkedGroup)
        {
            Id = linkedGroup.Id;
            Name = linkedGroup.Name;
            IsAuthorized = linkedGroup.AccessToken.HasValue();
            ExpirationDate = linkedGroup.ExpirationDate.ToString();
        }
    }
}