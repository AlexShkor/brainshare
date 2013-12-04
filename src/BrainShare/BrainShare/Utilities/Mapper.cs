using BrainShare.Documents;

namespace BrainShare.Utilities
{
    public static class Mapper
    {
        //Todo: implement necessary mappings when it will be need
        public static User MapShellUser(this ShellUser shellUser)
        {
            if (shellUser == null) return null;
            return new User();
        }
    }
}