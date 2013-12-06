using BrainShare.Authentication;
using BrainShare.Documents;

namespace BrainShare.Utilities
{
    public static class Mapper
    {
        public static CommonUser MapShellUser(this ShellUser shellUser)
        {
            if (shellUser == null)
                return null;

            return new CommonUser
            {
                Email = shellUser.Email,
                Id = shellUser.Id,
                IsShell = true,
                OriginalType = typeof(ShellUser),
                FullName = shellUser.Name,
                AvatarUrl = shellUser.AvatarUrl,
                Password = shellUser.Password,
                Salt =  shellUser.Salt
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
                OriginalType = typeof(User),
                FullName = user.FullName,
                AvatarUrl = user.AvatarUrl,
                Password = user.Password,
                Salt = user.Salt
            };
        }

    }
}