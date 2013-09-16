using System.ComponentModel.DataAnnotations;

namespace BrainShare.Controllers
{
    public class RegisterViewModel
    {
        private const string LocalityErrorMessage = "Вы допустили ошибку при выборе города";

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }


        [Required(ErrorMessage = LocalityErrorMessage)]
        public string original_address { get; set; }
        [Required(ErrorMessage = LocalityErrorMessage)]
        public string formatted_address { get; set; }
        [Required(ErrorMessage = LocalityErrorMessage)]
        public string country { get; set; }
        [Required(ErrorMessage = LocalityErrorMessage)]
        public string locality { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}