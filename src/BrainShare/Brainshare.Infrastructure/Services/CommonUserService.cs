using System;
using BrainShare.Infrastructure.Utilities;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Services
{
    public class CommonUserService :ICommonUserService
    {
        private readonly ShellUserService _shellUserService;
        private readonly UsersService _usersService;

        public CommonUserService(ShellUserService shellUserService, UsersService usersService)
        {
            _shellUserService = shellUserService;
            _usersService = usersService;
        }

        public CommonUser GetUserByEmail(string email)
        {
            var result = _usersService.GetUserByEmail(email).MapUser();
            return result ?? _shellUserService.GetUserByEmail(email).MapShellUser();
        }

        public CommonUser GetById(string id)
        {
            var result = _usersService.GetById(id).MapUser();
            return result ?? _shellUserService.GetById(id).MapShellUser();
        }

        public object GetUserByType(Type userType, string Id)
        {
            if (typeof (UsersService).Equals(userType))
            {
                return _usersService.GetById(Id);
            }
            if (typeof (ShellUserService).Equals(userType))
            {
                return _shellUserService.GetById(Id);
            }
            return null;
        }
    }
}