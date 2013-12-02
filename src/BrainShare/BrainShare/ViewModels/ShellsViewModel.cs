using System.Collections.Generic;
using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class ShellsViewModel
    {
        public IEnumerable<PublicShell> Shells { get; set; }

        public ShellsViewModel(IEnumerable<PublicShell> shells)
        {
            Shells = shells;
        }
    }
}