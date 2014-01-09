namespace BrainShare.Domain.Documents.Data
{
    public class AddressData
    {
        public string Original { get; set; }
        public string Formatted { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }

        public bool IsValid { get; set; }

        public AddressData(string originalAddress, string formattedAddress, string country, string locality)
        {
            Original = originalAddress;
            Formatted = formattedAddress;
            Country = country;
            Locality = locality;
            IsValid = true;
        }

        public AddressData()
        {
            
        }

        public AddressData(string vkCountry, string vkCity)
            : this(string.Format("{0},{1}", vkCountry, vkCity), string.Format("{0},{1}", vkCountry, vkCity),vkCountry,vkCity)
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