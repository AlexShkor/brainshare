using System.Collections.Generic;

namespace BrainShare.ViewModels
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