using System.Collections.Generic;
using Brainshare.Infrastructure.Aggregates.User.Data;
using Brainshare.Infrastructure.Aggregates.User.Events;
using Brainshare.Infrastructure.Platform.Domain;

namespace Brainshare.Infrastructure.Aggregates.User
{
    public sealed class UserState : AggregateState
    {
        public string Id { get; private set; }
        public bool UserWasDeleted { get; set; }

        public List<string> ExpertConnections { get; set; }

        public List<string> ExpertFollows { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Location { get; set; }

        public string Headline { get; set; }

        public string Department { get; set; }

        public bool Activated { get; set; }

        public UserState()
        {

            ExpertConnections = new List<string>();
            ExpertFollows = new List<string>();
            UserRoles = new List<UserRole>();
            Activated = true;

            On((UserCreated e) =>
            {
                Id = e.Id;
                FirstName = e.FirstName;
                LastName = e.LastName;
            });
            On((UserMarkedAsDeleted e) => UserWasDeleted = true);
            On((ExpertConnectRequestSent e) =>
                {
                    ExpertConnections.Add(e.Id);
                });
            On((ExpertFollowed e) =>
                {
                    ExpertFollows.Add(e.FollowedUserId);
                });
            On((ExpertUnfollowed e) =>
                {
                    ExpertFollows.Remove(e.FollowingUserId);
                });

            On((UserRoleAdded e) =>
                {
                    UserRoles.Add(e.Role);
                });
            On((UserRoleRemoved e) =>
                {
                    UserRoles.Remove(e.Role);
                });

            On((UserFirstLastNameChanged e) =>
            {
                FirstName = e.FirstName;
                LastName = e.LastName;
            });

            On((UserDepartmentChanged e) =>
            {
                Department = e.Department;
            });            
            
            On((UserActivated e) =>
            {
                Activated = true;
            });            
            
            On((UserDeactivated e) =>
            {
                Activated = false;
            });

        }

        public bool IsExpertConnectionExist(string connectedUserId)
        {
            return ExpertConnections.Contains(connectedUserId);
        }

        public bool IsExpertFollowExist(string followUserId)
        {
            return ExpertFollows.Contains(followUserId);
        }

        public bool IsUserHasRole(UserRole role)
        {
            return UserRoles.Contains(role);

        }
    }
}