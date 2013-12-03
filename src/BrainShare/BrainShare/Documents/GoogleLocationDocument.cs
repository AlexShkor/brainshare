using System.Collections.Generic;

// implement modelbinder
namespace BrainShare.Documents
{
    public class GoogleLocationDocument
    {
        public GoogleLocationDocument()
        {
            AddressComponents = new List<AddressComponent>();
            Types = new List<string>();
            Geometry = new Geometry();
        }
        public List<AddressComponent> AddressComponents { get; set; }
        public List<string> Types { get; set; }
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class AddressComponent
    {
        public AddressComponent()
        {
            Types = new List<string>();
        }
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; } 
    }

    public class Geometry
    {
        public Geometry()
        {
            Location = new Location();
            ViewPort = new ViewPort();
        }
        public ViewPort ViewPort { get; set; }
        public Location Location { get; set; }
        public string LocationType { get; set; }
    }

    public class ViewPort
    {
        public ViewPort()
        {
            NorthEast = new Location();
            SouthWest = new Location();
        }
        public Location NorthEast { get; set; }
        public Location SouthWest { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}