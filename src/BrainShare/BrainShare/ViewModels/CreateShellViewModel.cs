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

        [Required(ErrorMessage = "Пожалуйста, укажите улицу")]
        public string Route { get; set; }
        [Required(ErrorMessage = "Пожалуйста, укажите дом")]
        public string StreetNumber { get; set; }
        [Required(ErrorMessage = "Пожалуйста, укажите страну")]
        public string Country { get; set; }

        public string Lng { get; set; }
        public string Lat { get; set; }

        public string Name { get; set; }
        public string LocalPath { get; set; }    
    }
}