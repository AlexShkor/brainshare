using System.ComponentModel.DataAnnotations;
using BrainShare.Domain.Documents;
using BrainShare.ViewModels.Base;

namespace BrainShare.ViewModels
{
    public class EditShellProfileViewModel : BaseViewModel
    {
        public EditShellProfileViewModel()
        {

        }

        public EditShellProfileViewModel(ShellUser shellUser)
        {
            var address = shellUser.ShellAddressData;

            FormattedAddress = address.Formatted;
            Country = address.Country;
            Route = address.Route;
            Lat = address.Location.Latitude;
            Lng = address.Location.Longitude;
            LocalPath = address.LocalPath;
            Name = shellUser.Name;
        }


        public string FormattedAddress { get; set; }

        [Required(ErrorMessage = "Пожалуйста, дайте имя вашей полке")]
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
    }
}