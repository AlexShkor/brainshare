using System;
using System.ComponentModel.DataAnnotations;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class RegisterViewModel:BaseViewModel
    {

        private const string LocalityErrorMessage = "Пожалуйста, выберите город из списка";
        private const string EmptyErrorMessage = " "; // white space is important!

        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        [StringLength(50, ErrorMessage = "Имя должен содержать не более 50-ти символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        [StringLength(50, ErrorMessage = "Фамилия должна содержать не более 50-ти символов")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Укажите город")]
        public string original_address { get; set; }
        [Required(ErrorMessage = EmptyErrorMessage)]
        public string formatted_address { get; set; }
        [Required(ErrorMessage = EmptyErrorMessage)]
        public string country { get; set; }
        [Required(ErrorMessage = LocalityErrorMessage)]
        public string locality { get; set; }

        [Required(ErrorMessage = "Введите e-mail адресс")]
        [EmailAddress(ErrorMessage = "Проверьте правильность вашего e-mail адресса")]
        [StringLength(100, ErrorMessage = "Почта должна состоять не более чем из 100 символов")]
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
