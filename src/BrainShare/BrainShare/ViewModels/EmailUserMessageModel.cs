using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class EmailUserMessageModel
    {
        public string Message { get; set; }
        public User Sender { get; set; }
    }
}