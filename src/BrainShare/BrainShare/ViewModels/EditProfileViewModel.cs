using System.ComponentModel.DataAnnotations;
using BrainShare.ViewModels.Base;
using Brainshare.Infrastructure.Documents;

namespace BrainShare.ViewModels
{
    public class EditProfileViewModel : BaseViewModel
    {
        private const string LocalityErrorMessage = "Пожалуйста, выберите город из списка";
        private const string EmptyErrorMessage = " "; // white space is important!

        public EditProfileViewModel()
        {

        }

        public EditProfileViewModel(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            original_address = user.Address.Original;
            formatted_address = user.Address.Formatted;
            country = user.Address.Country;
            locality = user.Address.Locality;
            Info = user.Info;
        }
        
        [Required(ErrorMessage = "Введите имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        
        [Required(ErrorMessage = "Укажите город")]
        public string original_address { get; set; }
        [Required(ErrorMessage = EmptyErrorMessage)]
        public string formatted_address { get; set; }
        [Required(ErrorMessage = EmptyErrorMessage)]
        public string country { get; set; }
        [Required(ErrorMessage = LocalityErrorMessage)]
        public string locality { get; set; }

        public string Info { get; set; }
    }
}