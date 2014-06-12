using System.ComponentModel.DataAnnotations;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class ChangePasswordModel:BaseViewModel
    {
        public bool IsFacebokAccountWithoutPassword { get; set; }

        public bool DoesNotHaveEmail { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать не менее {2}-ти символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}