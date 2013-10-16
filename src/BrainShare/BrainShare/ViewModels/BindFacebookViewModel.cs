using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BrainShare.ViewModels
{
    public class BindFacebookViewModel
    {
        public string Email { get; set; }
        [Required(ErrorMessage = "Введите пароль")]
        public string Password { get; set; }
        public string FacebookId { get; set; }
    }
}