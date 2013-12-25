using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrainShare.Documents;
using Brainshare.Infrastructure.Documents;

namespace BrainShare.ViewModels
{
    public class ExchangeConfirmViewModel
    {
        public User You { get; set; }
        public User He { get; set; }
        public Book YourBook { get; set; }
        public Book HisBook { get; set; }
    }
}