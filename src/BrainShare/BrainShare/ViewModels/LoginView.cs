using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrainShare.ViewModels
{
    public class LoginView
    {
        [Required(ErrorMessage = "Enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your пароль")]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }

    }
}