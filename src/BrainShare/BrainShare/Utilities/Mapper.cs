using BrainShare.Authentication;
using BrainShare.Documents;

namespace BrainShare.Utilities
{
    public static class Mapper
    {
        //Todo: implement necessary mappings when it will be need
        //public static User MapShellUser(this ShellUser shellUser)
        //{
        //    if (shellUser == null) 
        //        return null;

        //    return new User
        //        {
        //            Email = shellUser.Email
        //        };
        //}

        public static CommonUser MapShellUser(this ShellUser shellUser)
        {
            if (shellUser == null)
                return null;

            return new CommonUser
            {
                Email = shellUser.Email,
                Id = shellUser.Id,
                IsShell = true,
            };
        }

        public static CommonUser MapUser(this User user)
        {
            if (user == null)
                return null;

            return new CommonUser
            {
                Email = user.Email,
                Id = user.Id,
                IsShell = false,
            };
        }

    }
}