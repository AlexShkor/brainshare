using System;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infrastructure.Utilities;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Services;

namespace BrainShare.Services
{
    public class CommonUserService :ICommonUserService
    {
        private readonly UsersService _usersService;

        public CommonUserService(UsersService usersService)
        {
            _usersService = usersService;
        }

        public CommonUser GetUserByLoginServiceInfo(LoginServiceTypeEnum loginServiceType, string serviceId)
        {
            return  _usersService.GetUserByLoginServiceInfo(loginServiceType, serviceId).MapUser();
        }

        public CommonUser GetUserByEmail(string email)
        {
            return _usersService.GetUserByEmail(email).MapUser();
        }

        public CommonUser GetUserByVkId(string id)
        {
            return _usersService.GetUserByVkId(id).MapUser();
        }

        public CommonUser GetUserByFacebookId(string id)
        {
            return _usersService.GetUserByFbId(id).MapUser();
        }

        public CommonUser GetById(string id)
        {
            return _usersService.GetById(id).MapUser();
        }
    }
}