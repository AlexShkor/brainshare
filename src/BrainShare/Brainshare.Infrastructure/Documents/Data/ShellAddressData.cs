using BrainShare.Infostructure;
using BrainShare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Documents.Data;

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