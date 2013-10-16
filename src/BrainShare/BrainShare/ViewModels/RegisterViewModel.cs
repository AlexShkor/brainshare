using System.ComponentModel.DataAnnotations;

namespace BrainShare.Controllers
{
    public class RegisterViewModel
    {
        private const string LocalityErrorMessage = "Пожалуйста, выберите город из списка";

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Укажите город")]
        public string original_address { get; set; }
        //[Required(ErrorMessage = LocalityErrorMessage)]
        public string formatted_address { get; set; }
        //[Required(ErrorMessage = LocalityErrorMessage)]
        public string country { get; set; }
        //[Required(ErrorMessage = LocalityErrorMessage)]
        public string locality { get; set; }

        [Required(ErrorMessage = "Введите e-mail адресс")]
        [EmailAddress(ErrorMessage = "Проверьте правильность вашего e-mail адресса")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать не менее {2}-ти символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
