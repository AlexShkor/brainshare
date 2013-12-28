using System.Collections.Generic;
using System.Linq;
using BrainShare.Domain.Documents;

namespace BrainShare.ViewModels
{
    public class AllThreadsViewModel
    {
        public List<ThreadItemViewModel> Items { get; set; } 

        public AllThreadsViewModel(IEnumerable<Thread> threads, string me, User user)
        {
            Items = threads.Select(x => new ThreadItemViewModel(x,me, user)).ToList();
        }
    }
}