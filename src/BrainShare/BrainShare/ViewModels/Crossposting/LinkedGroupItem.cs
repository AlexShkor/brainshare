using BrainShare.Domain.Documents;
using RestSharp.Extensions;

namespace BrainShare.Controllers
{
    public class LinkedGroupItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsAuthorized { get; set; }

        public string StatusClass { get; set; }

        public string Status { get; set; }
        public bool IsActive { get; set; }
        public bool CanPostAll { get; set; }

        public LinkedGroupItem(LinkedGroup linkedGroup)
        {
            Id = linkedGroup.Id;
            Name = linkedGroup.Name;
            CanPostAll = !linkedGroup.AllBooksPosted;
            IsAuthorized = linkedGroup.AccessToken.HasValue();
            IsActive = linkedGroup.IsActive;
            Status = linkedGroup.IsActive ? "Вкл" : "Выкл";
            StatusClass = linkedGroup.IsActive ? "text-success" : "text-error";
        }
    }
}