using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BrainShare.Documents;

namespace BrainShare.Authentication
{
    public interface IUserProvider
    {
        User User { get; set; }
    }
}