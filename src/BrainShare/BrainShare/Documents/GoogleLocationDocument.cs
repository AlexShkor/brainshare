using System.Collections.Generic;

// implement modelbinder
namespace BrainShare.Documents
{
    public class GoogleLocationDocument
    {
        public GoogleLocationDocument()
        {
            Address_Components = new List<AddressComponent>();
            Types = new List<string>();
            Geometry = new Geometry();
        }

        public List<AddressComponent> Address_Components { get; set; }
        public List<string> Types { get; set; }
        public string Formatted_Address { get; set; }
        public Geometry Geometry { get; set; }
    }

    public class AddressComponent
    {
        public AddressComponent()
        {
            Types = new List<string>();
        }
        public string Long_Name { get; set; }

        public string Short_Name { get; set; }
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
        public string Location_Type { get; set; }
    }

    public class ViewPort
    {
        public ViewPort()
        {
            North_East = new Location();
            South_West = new Location();
        }

        public Location North_East { get; set; }
        public Location South_West { get; set; }
    }

    public class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}