using System;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Documents.Data;

namespace Brainshare.Infrastructure.Services
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

        public CommonUser GetUserByLoginServiceInfo(LoginServiceTypeEnum loginServiceType, string serviceId)
        {
            var result = _usersService.GetUserByLoginServiceInfo(loginServiceType, serviceId).MapUser();
            return result ?? _shellUserService.GetUserByLoginServiceInfo(loginServiceType, serviceId).MapShellUser();
        }

        public CommonUser GetUserByEmail(string email)
        {
            var result = _usersService.GetUserByEmail(email).MapUser();
            return result ?? _shellUserService.GetUserByEmail(email).MapShellUser();
        }

        public CommonUser GetUserByVkId(string id)
        {
            var result = _usersService.GetUserByVkId(id).MapUser();
            return result ?? _shellUserService.GetUserByVkId(id).MapShellUser();
        }

        public CommonUser GetUserByFacebookId(string id)
        {
            var result = _usersService.GetUserByFbId(id).MapUser();
            return result ?? _shellUserService.GetUserByFbId(id).MapShellUser();
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