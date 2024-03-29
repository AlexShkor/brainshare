﻿using BrainShare.Domain.Documents;
using Brainshare.Infrastructure.Authentication;

namespace BrainShare.Infrastructure.Utilities
{
    public static class Mapper
    {
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
                Salt = user.Salt,
                IsFacebookAccount = user.IsFacebookAccount,
            //    LoginServices = user.LoginServices
            };
        }

    }
}