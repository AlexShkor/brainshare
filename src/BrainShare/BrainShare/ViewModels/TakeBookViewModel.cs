using System.Collections.Generic;
using BrainShare.ViewModels;

namespace BrainShare.Controllers
{
    public class TakeBookViewModel
    {
        public string Id { get; set; }

        public BookViewModel Book { get; set; }

        public List<UserItemViewModel> Owners { get; set; }

        public TakeBookViewModel()
        {
            Owners = new List<UserItemViewModel>();
        }
    }
}