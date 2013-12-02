using System.ComponentModel.DataAnnotations;
using BrainShare.Documents;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class CreateShellViewModel:BaseViewModel
    {
        public CreateShellViewModel()
        {
            GoogleLocationModel = new GoogleLocationDocument();
        }

        public CreateShellViewModel(string formattedAddress):this()
        {
            GoogleLocationModel.Formatted_Address = formattedAddress;
        }

        [Required(ErrorMessage = "Введите название")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите точное расположение в здании")]
        public string LocalPath { get; set; }

        [Required(ErrorMessage = "сервисы google не отвечают. Попробуйте позже")]
        public GoogleLocationDocument GoogleLocationModel { get; set; }
    }
}