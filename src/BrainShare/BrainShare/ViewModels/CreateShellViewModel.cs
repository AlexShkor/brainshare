using System.ComponentModel.DataAnnotations;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class CreateShellViewModel:BaseViewModel
    {
        public CreateShellViewModel()
        {

        }

        public CreateShellViewModel(string formattedAddress)
        {
            FormattedAddress = formattedAddress;
        }
        public string FormattedAddress { get; set; }
        public string Name { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите страну")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите улицу")]
        public string Route { get; set; }

        [Required(ErrorMessage = "Пожалуйста, укажите дом")]
        public int StreetNumber { get; set; }

        public string LocalPath { get; set; }   

        public double Lng { get; set; }
        public double Lat { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать не менее {2}-ти символов", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите e-mail адресс")]
        [EmailAddress(ErrorMessage = "Проверьте правильность вашего e-mail адресса")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтвердите пароль")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
 
    }
}