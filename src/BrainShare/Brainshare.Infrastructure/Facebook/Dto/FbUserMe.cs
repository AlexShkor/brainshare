namespace Brainshare.Infrastructure.Facebook.Dto
{
    public class FbUserMe
    {
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public FbLocation location { get; set; }
        public string id { get; set; }

        public FbUserMe()
        {
            //Default location
            location = new FbLocation()
            {
                name = "Minsk, Belarus"
            };
        }

        public class FbLocation
        {
            public string name { get; set; }
        }
    }
}