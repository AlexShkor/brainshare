using BrainShare.Infostructure;

namespace BrainShare.Documents.Data
{
    public class ShellAddressData:AddressData
    {
        public string Route { get; set; }
        public int StreetNumber { get; set; }
        public string LocalPath { get; set; }
        public Location Location { get; set; }
    }
}