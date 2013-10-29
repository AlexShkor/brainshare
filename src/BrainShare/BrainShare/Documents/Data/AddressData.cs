using BrainShare.ViewModels;

namespace BrainShare.Documents
{
    public class AddressData
    {
        public string Original { get; set; }
        public string Formatted { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }

        public bool IsValid { get; set; }

        public AddressData(RegisterViewModel model)
        {
            Original = model.original_address;
            Formatted = model.formatted_address;
            Country = model.country;
            Locality = model.locality;
            IsValid = true;
        }

        public AddressData()
        {
            
        }

        public AddressData(string fbLocationData)
        {
            Original = fbLocationData;
            Formatted = fbLocationData;
            try
            {
                var arr = fbLocationData.Split(',');
                Country = arr[1].Trim();
                Locality = arr[0].Trim();
                IsValid = true;
            }
            catch
            {
                IsValid = false;
            }
        }

        public string GetCityAndCountry()
        {
            return string.Format("{0}, {1}", Locality, Country);
        }
    }
}