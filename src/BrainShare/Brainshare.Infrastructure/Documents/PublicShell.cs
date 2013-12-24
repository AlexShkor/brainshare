using BrainShare.Documents.Data;

namespace BrainShare.Documents
{
    public class ShellUser:BaseUser
    {
        public ShellAddressData ShellAddressData { get; set; }
        public string Name { get; set; }
        public override string UserType { get { return "ShellUser"; } }
    }
}